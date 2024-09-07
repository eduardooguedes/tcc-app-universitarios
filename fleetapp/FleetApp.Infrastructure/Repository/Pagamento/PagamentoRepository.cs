using Dashdine.Domain.Domain.Integracoes.PagSeguro;
using Dashdine.Domain.Domain.Pagamento;
using Dashdine.Domain.Domain.Pedido;
using Dashdine.Domain.Entitys;
using Dashdine.Domain.Interface.Pagamento;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Dashdine.Infrastructure.Repository.Pagamento;

public sealed class PagamentoRepository(IConfiguration configuration, ISituacaoPagamentoRepository situacaoPagamentoRepository, ITipoPagamentoRepository tipoPagamentoRepository) : BaseRepository<Domain.Entitys.Pagamento>(configuration), IPagamentoRepository
{
    private readonly PagSeguroClient pagSeguroClient = ObterClient(configuration);
    private const string MENSAGEM_SERVICO_INDISPONIVEL = "Pagamento fora do ar. Tente novamente mais tarde.";

    private static PagSeguroClient ObterClient(IConfiguration configuration)
    {
        var urlBase = configuration["pagamentos:pagseguro:urlBase"]?.ToString() ?? throw new ApplicationException(MENSAGEM_SERVICO_INDISPONIVEL);
        var authorization = configuration["pagamentos:pagseguro:authorization"]?.ToString() ?? throw new ApplicationException(MENSAGEM_SERVICO_INDISPONIVEL);
        var endpointPedido = configuration["pagamentos:pagseguro:endpointPedido"]?.ToString() ?? throw new ApplicationException(MENSAGEM_SERVICO_INDISPONIVEL);
        var endpointCancelarPagamento = configuration["pagamentos:pagseguro:endpointCancelarPagamento"]?.ToString() ?? throw new ApplicationException(MENSAGEM_SERVICO_INDISPONIVEL);
        return new PagSeguroClient(urlBase, authorization, endpointPedido, endpointCancelarPagamento);
    }

    public async Task<PagamentoPedidoDomain?> SalvarPagamentoPix(PedidoDomain pedido, DateTime dataHora, int segundosParaPagar)
    {
        var area = pedido.Cliente.Celular.Length >= 10 ? pedido.Cliente.Celular[..2] : string.Empty;
        var numero = pedido.Cliente.Celular.Length >= 10 ? pedido.Cliente.Celular[2..] : pedido.Cliente.Celular;
        var dataHoraExpiracao = dataHora.AddSeconds(segundosParaPagar);

        var pedidoPagSeguro = new PedidoPagSeguroDomain()
        {
            IdPedido = pedido.Id.ToString(),
            Cliente = new()
            {
                Cpf = pedido.Cliente.Cpf,
                Email = pedido.Cliente.Email,
                Nome = pedido.Cliente.Nome,
                Telefones = [new TelefonePagSeguroDomain() { Pais = "55", Area = area, Numero = numero }]
            },
            QrCodes = [new QrCodePagSeguroDomain() { Quantia = new() { ValorDecimal = pedido.ValorTotal }, DataExpiracao = dataHoraExpiracao }],
            UrlsParaNotificacao = [configuration["pagamentos:webhookPagamento"]!]
        };

        var resposta = await pagSeguroClient.SalvarPedidoQrCode(pedidoPagSeguro);
        if (!resposta.IsSuccessStatusCode)
        {
            var respostaErro = await resposta.Content.ReadAsStringAsync();
            return null;
        }

        pedidoPagSeguro = JsonConvert.DeserializeObject<PedidoPagSeguroDomain>(await resposta.Content.ReadAsStringAsync());
        if (pedidoPagSeguro is null)
            return null;

        var imagemQrCode = pedidoPagSeguro.QrCodes.FirstOrDefault()?.Links.FirstOrDefault(l => l.Relacao.Equals("QRCODE.PNG"))?.Href;
        var textoPix = pedidoPagSeguro.QrCodes.FirstOrDefault()?.Texto;
        var idQrCode = pedidoPagSeguro.QrCodes.FirstOrDefault()?.Id;

        if (textoPix is null)
            return null;

        var tipoPagamentoPix = await tipoPagamentoRepository.ObterTipoPagamentoPix();
        if (tipoPagamentoPix is null)
            return null;

        var situacaoAguardando = await situacaoPagamentoRepository.ObterSituacaoAguardando();

        var idPagamento = Guid.NewGuid();
        await UnitOfWork.Pagamentos.AddAsync(
            new Domain.Entitys.Pagamento()
            {
                Id = idPagamento,
                IdPedido = pedido.Id,
                IdPedidoGateway = pedidoPagSeguro.IdPedidoPagSeguro!,
                IdCobrancaGateway = null,
                DataHora = dataHora,
                DataHoraAtualizado = null,
                DataHoraExpiracao = dataHoraExpiracao,
                IdCartao = null,
                IdSituacao = situacaoAguardando.Id,
                IdTipo = tipoPagamentoPix.Id,
                IdQrCodeGateway = idQrCode,
                TextoPix = textoPix,
                Valor = pedido.ValorTotal,
                ImagemQrCode = imagemQrCode,
            });
        await SaveChangesAsync();

        return new(idPagamento, situacaoAguardando, tipoPagamentoPix, pedido.ValorTotal, dataHora, null, dataHoraExpiracao, null, textoPix, imagemQrCode);
    }

    public async Task<PagamentoPedidoDomain?> ObterUltimoPagamento(PedidoDomain pedido, DateTime dataHoraAtual)
    {
        var ultimoPagamento = await UnitOfWork.Pagamentos
            .Include(pagamento => pagamento.IdSituacaoNavigation)
            .Include(pagamento => pagamento.IdTipoNavigation)
            .Where(pagamento => pagamento.IdPedido.Equals(pedido.Id))
            .OrderByDescending(pagamento => pagamento.DataHora)
            .FirstOrDefaultAsync();

        if (ultimoPagamento is null)
            return null;

        if (ultimoPagamento.DataHoraExpiracao is null)
            return ToDomain(ultimoPagamento);

        if (ultimoPagamento.DataHoraExpiracao.Value <= dataHoraAtual)
            return await AtualizarPagamentoExpirado(ultimoPagamento, dataHoraAtual);

        return ToDomain(ultimoPagamento);
    }

    public async Task<bool> Cancelar(PagamentoPedidoDomain pagamento, DateTime dataHoraAtual)
    {
        var entidade = await UnitOfWork.Pagamentos.Where(pag => pag.Id.Equals(pagamento.Id)).FirstOrDefaultAsync();
        if (entidade is null) return false;

        if (entidade.IdCobrancaGateway is null)
        {
            var idCobranca = await obterIdCobrancaGateway(entidade.IdPedidoGateway);
            if (idCobranca is null)
                return false;

            entidade.IdCobrancaGateway = idCobranca;
            await SaveChangesAsync();
        }

        var quantia = new QuantiaPagSeguroDomain() { ValorDecimal = entidade.Valor };
        var pagamentoCancelado = await pagSeguroClient.CancelarPagamentoPedido(entidade.IdCobrancaGateway, new { amount = quantia });
        if (pagamentoCancelado is null)
            return false;

        if (!pagamentoCancelado.IsSuccessStatusCode)
        {
            var respostaErro = await pagamentoCancelado.Content.ReadAsStringAsync();
            return false;
        }

        var respostaPagamentoCancelado = JsonConvert.DeserializeObject<CobrancaPagSeguroDomain>(await pagamentoCancelado.Content.ReadAsStringAsync());
        if (respostaPagamentoCancelado is null)
            return false;

        if (respostaPagamentoCancelado.Status != "CANCELED")
            return false;

        await AtualizarPagamentoCancelado(entidade, dataHoraAtual);
        return true;
    }

    private async Task<string?> obterIdCobrancaGateway(string idPedidoGateway)
    {
        var respostaPedidoAtualizado = await pagSeguroClient.ObterPedidoAtualizado(idPedidoGateway);
        if (respostaPedidoAtualizado is null) return null;

        var pedidoPagSeguro = JsonConvert.DeserializeObject<PedidoPagSeguroDomain>(await respostaPedidoAtualizado.Content.ReadAsStringAsync());
        if (pedidoPagSeguro is null)
            return null;

        if (!pedidoPagSeguro.Cobrancas.Any())
            return null;

        return pedidoPagSeguro.Cobrancas.First().Id;
    }

    private async Task AtualizarPagamentoCancelado(Domain.Entitys.Pagamento pagamento, DateTime dataHoraAtual)
    {
        pagamento.IdSituacao = (await situacaoPagamentoRepository.ObterSituacaoCancelado()).Id;
        pagamento.DataHoraAtualizado = dataHoraAtual;
        UnitOfWork.Pagamentos.Update(pagamento);
        await SaveChangesAsync();
    }

    private async Task<PagamentoPedidoDomain> AtualizarPagamentoExpirado(Domain.Entitys.Pagamento pagamento, DateTime dataHoraAtual)
    {
        var situacaoExpirado = await situacaoPagamentoRepository.ObterSituacaoExpirado();
        var domain = ToDomain(pagamento) with { Situacao = situacaoExpirado };

        pagamento.IdSituacao = situacaoExpirado.Id;
        pagamento.DataHoraAtualizado = dataHoraAtual;

        UnitOfWork.Pagamentos.Update(pagamento);

        await SaveChangesAsync();
        return domain;
    }

    private PagamentoPedidoDomain ToDomain(Domain.Entitys.Pagamento pagamento)
        => new PagamentoPedidoDomain(pagamento.Id, ToDomain(pagamento.IdSituacaoNavigation), new TipoPagamentoDomain(pagamento.IdTipoNavigation.Id, pagamento.IdTipoNavigation.Descricao, pagamento.IdTipoNavigation.CartaoCredito, pagamento.IdTipoNavigation.Pix), pagamento.Valor, pagamento.DataHora, pagamento.DataHoraAtualizado, pagamento.DataHoraExpiracao, pagamento.IdCartao, pagamento.TextoPix, pagamento.ImagemQrCode);

    private SituacaoPagamentoDomain ToDomain(SituacaoPagamento situacaoPagamento) => new SituacaoPagamentoDomain(situacaoPagamento.Id, situacaoPagamento.Descricao, situacaoPagamento.Aguardando, situacaoPagamento.EmAnalise, situacaoPagamento.Pago, situacaoPagamento.Cancelado, situacaoPagamento.Rejeitado, situacaoPagamento.Expirado, situacaoPagamento.EstornarAoCancelarPedido);
}
