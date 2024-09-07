using Dashdine.Domain.Domain.Pagamento;
using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Domain.Interface.Pagamento;
using Dashdine.Domain.Interface;
using Dashdine.Service.Enums;
using Dashdine.Service.Exceptions.Cliente.Pedido;
using Dashdine.Service.Models.Cliente.Pedido;
using Dashdine.Service.Interface.Cliente;
using Dashdine.Domain.Interface.Pedido;
using Dashdine.Service.Extensions;
using Dashdine.Service.Models.Pedido;

namespace Dashdine.Service.Services.Cliente;

public sealed class PagamentoPedidoClienteService(IPedidoRepository pedidoRepository, ISituacaoPedidoRepository situacaoPedidoRepository, IPagamentoRepository pagamentoRepository, IParametroRepository parametroRepository, ITimeZoneRepository timeZoneRepository, ISituacaoPagamentoRepository situacaoPagamentoRepository) : IPagamentoPedidoClienteService
{
    public async Task<ProjecaoDePagamentoDoPedidoDoCliente> PagarPedido(UsuarioAutenticado clienteAutenticado, Guid idPedido, DtoPagarPedido pagamento)
    {
        var pedido = await pedidoRepository.ObterPedido(idPedido) ?? throw new PedidoNaoEncontradoException();

        if (pedido.Cliente.Id != clienteAutenticado.Id) throw new PedidoNaoEncontradoException();

        if (!pedido.Situacao.PermitidoPagar)
            throw new ApplicationException("Não é mais permitido pagar esse pedido.");

        var situacaoAguardandoPagamento = await situacaoPedidoRepository.ObterSituacaoAguardandoPagamento();
        if (pedido.Situacao.Id != situacaoAguardandoPagamento.Id)
        {
            pedido = pedido with { Situacao = situacaoAguardandoPagamento };
            await pedidoRepository.SalvarCabecalhoPedido(pedido);
        }

        var timezone = (await timeZoneRepository.Obter(pedido.LocalizacaoCliente.Timezone));

        var pagamentoSolido = ValidarObterPagamentoSolido(await pagamentoRepository.ObterUltimoPagamento(pedido, timezone.DataHoraAtual));
        if (pagamentoSolido is not null)
            return new(pagamento.IdFormaDePagamento, pagamentoSolido.Tipo.Id, pagamentoSolido.Situacao.AsProjecao(), pagamentoSolido.ObterSegundosParaExpirar(timezone.DataHoraAtual), pagamentoSolido.LinkPagamento, pagamentoSolido.ImagemQrCode);

        if (pagamento.TipoDaFormaDePagamento.Equals((int)EnumTipoDeFormaDePagamento.Pix))
        {
            int segundosParaPagarPix = await parametroRepository.ObterSegundosPadraoExpiracaoPix();

            var pagamentoPedidoDomain = await pagamentoRepository.SalvarPagamentoPix(pedido, timezone.DataHoraAtual, segundosParaPagarPix) ?? throw new ApplicationException("Pix fora do ar. Por favor, tente novamente mais tarde.");

            return new(pagamento.IdFormaDePagamento, (int)EnumTipoDeFormaDePagamento.Pix, pagamentoPedidoDomain.Situacao.AsProjecao(), segundosParaPagarPix, pagamentoPedidoDomain.LinkPagamento, pagamentoPedidoDomain.ImagemQrCode);
        }

        //PagamentoPedidoDomain = pagamentoRepository.PagarViaCartao(cartaoDomain);

        return new(pagamento.IdFormaDePagamento, (int)EnumTipoDeFormaDePagamento.Cartao, null, null, null, null);
    }

    private static PagamentoPedidoDomain? ValidarObterPagamentoSolido(PagamentoPedidoDomain? ultimoPagamento)
    {
        if (ultimoPagamento is null)
            return null;

        if (ultimoPagamento.Situacao.Expirado)
            return null;

        if (ultimoPagamento.Situacao.Rejeitado)
            return null;

        return ultimoPagamento;
    }

    public async Task<ProjecaoDePagamentoDoPedidoDoCliente> ObterUltimoPagamentoDoPedido(UsuarioAutenticado clienteAutenticado, Guid idPedido)
    {
        var pedido = await pedidoRepository.ObterPedido(idPedido) ?? throw new PedidoNaoEncontradoException();

        if (pedido.Cliente.Id != clienteAutenticado.Id) throw new PedidoNaoEncontradoException();

        var timezone = (await timeZoneRepository.Obter(pedido.LocalizacaoCliente.Timezone));

        var ultimoPagamento = await pagamentoRepository.ObterUltimoPagamento(pedido, timezone.DataHoraAtual)
            ?? throw new ApplicationException("Pagamento não encontrado. Por favor, crie seu pedido novamente.");

        return new(ultimoPagamento.IdCartao, ultimoPagamento.Tipo.Id, ultimoPagamento.Situacao.AsProjecao(), ultimoPagamento.ObterSegundosParaExpirar(timezone.DataHoraAtual), ultimoPagamento.LinkPagamento, ultimoPagamento.ImagemQrCode);
    }

    public async Task CancelarPagamentoExcluirPedido(UsuarioAutenticado clienteAutenticado, Guid idPedido)
    {
        var pedido = await pedidoRepository.ObterPedido(idPedido) ?? throw new PedidoNaoEncontradoException();

        if (pedido.Cliente.Id != clienteAutenticado.Id) throw new PedidoNaoEncontradoException();

        var timezone = (await timeZoneRepository.Obter(pedido.LocalizacaoCliente.Timezone));

        var ultimoPagamento = await pagamentoRepository.ObterUltimoPagamento(pedido, timezone.DataHoraAtual);
        if (ultimoPagamento is not null && !await pagamentoRepository.Cancelar(ultimoPagamento, timezone.DataHoraAtual))
        {
            throw new ApplicationException("Não foi possível cancelar o pagamento do seu pedido. Tente novamente mais tarde.");
        }

        await pedidoRepository.CancelarPedido(pedido);
    }

    public async Task AtualizarPagamento(DtoPedidoPagSeguro pedidoPagSeguro)
    {
        if (!Guid.TryParse(pedidoPagSeguro.IdPedido, out var guidPedido))
            throw new PedidoNaoEncontradoException();

        var pedido = await pedidoRepository.ObterPedido(guidPedido) ?? throw new PedidoNaoEncontradoException();
        var timezone = await timeZoneRepository.Obter(pedido.LocalizacaoCliente.Timezone);

        var pagamento = await pagamentoRepository.ObterUltimoPagamento(pedido, timezone.DataHoraAtual) ?? throw new NenhumPagamentoEncontradoParaPedidoException();

        //OBTER SITUACAO DE ACORDO COM RESPOSTA DA PAG SEGURO
        //var situacaoAtualizada = await situacaoRepos

        //se existir, busca por status e atualiza

        //definir fluxo para cada status
    }
}
