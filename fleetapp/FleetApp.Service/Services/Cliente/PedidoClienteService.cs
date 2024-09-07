using Dashdine.CrossCutting.Extensions;
using Dashdine.Domain.Domain.Estabelecimento.HorarioDeFuncionamento;
using Dashdine.Domain.Domain.Pedido;
using Dashdine.Domain.Domain.Produto;
using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Domain.Interface;
using Dashdine.Domain.Interface.Cliente;
using Dashdine.Domain.Interface.Estabelecimento;
using Dashdine.Domain.Interface.Pagamento;
using Dashdine.Domain.Interface.Pedido;
using Dashdine.Domain.Interface.Produto;
using Dashdine.Service.Enums;
using Dashdine.Service.Exceptions.Cliente;
using Dashdine.Service.Exceptions.Cliente.Pedido;
using Dashdine.Service.Exceptions.Estabelecimento;
using Dashdine.Service.Extensions;
using Dashdine.Service.Interface.Cliente;
using Dashdine.Service.Models.Cliente.Pedido;
using Dashdine.Service.Models.Filtros;
using Dashdine.Service.Models.Produto;
using Microsoft.EntityFrameworkCore;

namespace Dashdine.Service.Services.Cliente;

public sealed class PedidoClienteService(IPedidoRepository pedidoRepository, IEnderecoClienteRepository enderecoClienteRepository, IEstabelecimentoRepository estabelecimentoRepository, ISituacaoPedidoRepository situacaoPedidoRepository, IProdutoRepository produtoRepository, ITimeZoneRepository timeZoneRepository, ICartoesClienteRepository cartoesClienteRepository, IPagamentoRepository pagamentoRepository) : IPedidoClienteService
{
    private const string MENSAGEM_PARA_RETIRAR_PEDIDO = "Informe seu nome ou telefone para retirar o pedido.";

    public async Task<ProjecaoDeInformacaoSobreOsPedidosDoCliente> ObterInformacoesSobreOsPedidosDoCliente(Guid idCliente) => new ProjecaoDeInformacaoSobreOsPedidosDoCliente(await pedidoRepository.ObterQuantidadeDePedidosEmDestaque(idCliente));

    public async Task<Guid> AdicionarAoPedidoEmAndamento(UsuarioAutenticado usuario, DtoAdicionarAoPedidoDoCliente adicionarAoPedido)
    {
        var localizacao = await enderecoClienteRepository.ObterLocalizacaoDoCliente(adicionarAoPedido.IdLocalizacaoCliente) ?? throw new LocalizacaoDoClienteNaoEncontradaException();
        var estabelecimento = await estabelecimentoRepository.ObterEstabelecimentoDoCliente(adicionarAoPedido.IdEstabelecimento) ?? throw new EstabelecimentoNaoEncontradoException();
        var situacaoEmAndamento = await situacaoPedidoRepository.ObterSituacaoEmAndamento();
        var dataHoraTimezone = await timeZoneRepository.Obter(localizacao.Timezone);

        var pedidoEmAndamentoDoEstabelecimento = await pedidoRepository.ObterPedidoEmAndamento(usuario.Id, adicionarAoPedido.IdEstabelecimento)
            ?? new PedidoDomain(Guid.NewGuid(), Numero: null, new ClienteDoPedidoDomain(usuario.Id, usuario.Nome, string.Empty, usuario.Email, usuario.Celular), localizacao, estabelecimento, situacaoEmAndamento, Destino: null, dataHoraTimezone.DataHoraAtual, DataHoraARetirar: null, Observacao: null, ObservacaoDoEstabelecimento: null, ValorTotal: 0, DataHoraRetirado: null, Produtos: []);

        var produtos = await ObterProdutosValidos(estabelecimento.Id, adicionarAoPedido.Produtos);
        if (pedidoEmAndamentoDoEstabelecimento.Produtos.Any())
            pedidoEmAndamentoDoEstabelecimento.Produtos.ToList().ForEach(produtos.Add);

        var pedidoEmAndamentoAtualizado = pedidoEmAndamentoDoEstabelecimento with { ValorTotal = ObterValorTotalPedido(produtos), Produtos = produtos };

        await pedidoRepository.SalvarPedido(pedidoEmAndamentoAtualizado);
        return pedidoEmAndamentoAtualizado.Id;
    }

    private async Task<IList<ProdutoDoPedidoDomain>> ObterProdutosValidos(Guid idEstabelecimento, IEnumerable<DtoDeProdutoDoPedidoDoCliente> produtosRequest)
    {
        var produtosDomain = new List<ProdutoDoPedidoDomain>();
        foreach (var produtoRequest in produtosRequest)
        {
            var produtoDomain = await produtoRepository.Obter(produtoRequest.IdProduto) ?? throw new ProdutoNaoEncontradoException(produtoRequest.Nome);
            if (produtoDomain.IdEstabelecimento != idEstabelecimento) throw new ProdutoNaoEncontradoException(produtoRequest.Nome);

            var idProdutoDoPedido = Guid.NewGuid();
            var adicionaisDomain = ObterAdicionaisValidos(idEstabelecimento, idProdutoDoPedido, produtoDomain, produtoRequest.Adicionais);
            produtosDomain.Add(
                new ProdutoDoPedidoDomain(
                    idProdutoDoPedido,
                    produtoRequest.IdProduto,
                    new SituacaoDeProdutoDomain(produtoDomain.Situacao.Id, produtoDomain.Situacao.Descricao),
                    new TipoDoProdutoDomain(produtoDomain.Tipo.Id, produtoDomain.Tipo.Descricao),
                    produtoDomain.Nome,
                    produtoDomain.Descricao,
                    produtoDomain.Imagem,
                    produtoRequest.Quantidade,
                    produtoDomain.Preco,
                    produtoDomain.Preco + (adicionaisDomain?.Select(p => p.PrecoTotal).Sum() ?? 0.0m),
                    ((produtoDomain.Preco + (adicionaisDomain?.Select(p => p.PrecoTotal).Sum() ?? 0.0m)) * produtoRequest.Quantidade).ArredondarPreco(),
                    adicionaisDomain));
        }
        return produtosDomain;
    }

    private static List<AdicionalDoProdutoDoPedidoDomain>? ObterAdicionaisValidos(Guid idEstabelecimento, Guid idProdutoDoPedido, ProdutoDomain produtoDomain, IEnumerable<DtoDeAdicionalDeProdutoDoPedidoDoCliente>? adicionaisRequest)
    {
        if (adicionaisRequest is null) return null;
        if (produtoDomain.Adicionais is null) throw new ProdutoSemAdicionaisException(produtoDomain.Nome);

        List<AdicionalDoProdutoDoPedidoDomain> adicionaisDomain = [];
        foreach (var adicionalRequest in adicionaisRequest.Where(a => a.Quantidade > 0))
        {
            var adicionalDomain = produtoDomain.Adicionais.Find(a => a.Id.Equals(adicionalRequest.Id))
                ?? throw new AdicionalNaoEncontradoException(adicionalRequest.Nome, produtoDomain.Nome);

            if (adicionalDomain.IdEstabelecimento != idEstabelecimento)
                throw new AdicionalNaoEncontradoException(adicionalRequest.Nome, produtoDomain.Nome);

            if (adicionalDomain.QuantidadeMaxima.HasValue && adicionalRequest.Quantidade > adicionalDomain.QuantidadeMaxima)
                throw new QuantidadeInformadaMaiorQuePermitidaParaAdicionalException(adicionalDomain.Nome!, produtoDomain.Nome, adicionalDomain.QuantidadeMaxima.Value);

            adicionaisDomain.Add(new AdicionalDoProdutoDoPedidoDomain(adicionalRequest.Id, idProdutoDoPedido, new(adicionalDomain.Situacao.Id, adicionalDomain.Situacao.Descricao), adicionalDomain.Nome!, adicionalRequest.Quantidade, adicionalDomain.Preco ?? 0, adicionalDomain.QuantidadeMaxima));
        }
        return adicionaisDomain;
    }

    public async Task CancelarPedido(UsuarioAutenticado cliente, Guid idPedido)
    {
        var pedido = await pedidoRepository.ObterPedido(idPedido)
            ?? throw new PedidoNaoEncontradoException();

        if (pedido.Cliente.Id != cliente.Id)
            throw new PedidoNaoEncontradoException();

        if (!await PermitidoCancelarPedido(pedido))
            throw new NaoEhPossivelCancelarPedidoException();

        if (pedido.Situacao.EstornarPagamentoAoCancelar)
            await EstornarPagamento(pedido);

        await pedidoRepository.CancelarPedido(pedido);
    }

    private async Task EstornarPagamento(PedidoDomain pedido)
    {
        var dataHoraAtual = (await timeZoneRepository.Obter(pedido.LocalizacaoCliente.Timezone)).DataHoraAtual;
        var ultimoPagamento = await pagamentoRepository.ObterUltimoPagamento(pedido, dataHoraAtual);
        if ((ultimoPagamento?.Situacao.EstornarAoCancelarPedido ?? false)
            && !await pagamentoRepository.Cancelar(ultimoPagamento, dataHoraAtual))
        {
            throw new ApplicationException("Não foi possível estornar seu pagamento. Tente novamente mais tarde.");
        }
    }

    private async Task<bool> PermitidoCancelarPedido(PedidoDomain pedido)
    {
        if (!pedido.Situacao.PermitidoCancelar)
            return false;

        if (pedido.Situacao.IntervaloMinimoEmHorasAntesDeRetirarParaCancelar is null)
            return true;

        if (pedido.DataHoraARetirar is null)
            return true;

        var horaAtualTimezonePedido = (await timeZoneRepository.Obter(pedido.LocalizacaoCliente.Timezone)).DataHoraAtual;
        if (horaAtualTimezonePedido > pedido.DataHoraARetirar.Value)
            return false;

        int intervaloEmHoras = pedido.Situacao.IntervaloMinimoEmHorasAntesDeRetirarParaCancelar.Value;
        return !(horaAtualTimezonePedido.AddHours(intervaloEmHoras) > pedido.DataHoraARetirar);
    }

    public async Task<IEnumerable<ProjecaoDeCategoriaParaPedidoDoClienteParaListagem>> ObterPedidosDoCliente(FiltrosRequest? filtros, Guid codigoCliente, IEnumerable<int> listaDeSituacoesDosPedidos, bool apenasPedidosRetirados = false)
    {
        var situacoesDosPedidos = await situacaoPedidoRepository.ObterSituacoes();
        foreach (var situacao in listaDeSituacoesDosPedidos)
        {
            if (!situacoesDosPedidos.Any(s => s.Id.Equals(situacao)))
                throw new SituacaoDePedidoNaoEncontradaException(situacao);
        }

        var pedidos =
            (await pedidoRepository.ObterPedidosDoCliente(filtros?.ToDomain() ?? new Domain.Filtros.FiltrosDomain(1, 60), codigoCliente, situacoesDosPedidos, apenasPedidosRetirados))
            .OrderByDescending(p => p.Situacao.PossuiCategoriaPropriaParaCliente)
            .ThenBy(p => p.Situacao.Id)
            .ThenBy(p => p.DataHoraARetirar);

        var situacaoAceito = await situacaoPedidoRepository.ObterSituacaoAceito();

        Dictionary<string, List<ProjecaoDePedidoDoClienteParaListagem>> categoriasParaListagem = [];
        foreach (var pedido in pedidos)
        {
            var dataHoraARetirar = pedido.DataHoraARetirar.ToDiaDaSemanaEDiaMes();
            var descricaoCategoria = pedido.Situacao.PossuiCategoriaPropriaParaCliente || dataHoraARetirar is null ? pedido.Situacao.Descricao : dataHoraARetirar;

            var apresentarMensagem = pedido.Situacao.Id.Equals(situacaoAceito.Id);

            var pedidoParaListagem = new ProjecaoDePedidoDoClienteParaListagem(pedido.Id, dataHoraARetirar, pedido.Situacao.AsProjecao(), pedido.ValorTotal, pedido.LocalizacaoCliente.AsProjecaoParaListagem(), DistanciaEmMetrosAteEstabelecimento: null, pedido.Produtos.Select(p => p.AsProjecaoParaListagem()), pedido.Estabelecimento.AsProjecao(), apresentarMensagem ? MENSAGEM_PARA_RETIRAR_PEDIDO : null);

            if (!categoriasParaListagem.ContainsKey(descricaoCategoria))
                categoriasParaListagem.Add(descricaoCategoria, []);
            categoriasParaListagem[descricaoCategoria].Add(pedidoParaListagem);
        }

        return categoriasParaListagem.Select(c => new ProjecaoDeCategoriaParaPedidoDoClienteParaListagem(c.Key, c.Value));
    }

    public async Task<ProjecaoDoPedidoDoCliente> ObterPedidoDoCliente(UsuarioAutenticado cliente, Guid idPedido)
    {
        var pedido = await pedidoRepository.ObterPedido(idPedido) ?? throw new PedidoNaoEncontradoException();
        if (pedido.Cliente.Id != cliente.Id) throw new PedidoNaoEncontradoException();

        var situacaoEmAndamento = await situacaoPedidoRepository.ObterSituacaoEmAndamento();
        var situacaoAceito = await situacaoPedidoRepository.ObterSituacaoAceito();
        var situacaoRetirado = await situacaoPedidoRepository.ObterSituacaoRetirado();

        bool permitidoAlterarProdutos = pedido.Situacao.Id.Equals(situacaoEmAndamento.Id);
        bool permitidoAlterarHorarioDeRetirada = permitidoAlterarProdutos;
        bool permitidoAlterarObservacao = permitidoAlterarHorarioDeRetirada;

        var pedidoAceito = pedido.Situacao.Id.Equals(situacaoAceito.Id);
        var permitidoCancelar = await PermitidoCancelarPedido(pedido);
        var permitidoRetirar = !permitidoCancelar && pedidoAceito;
        var permitidoPedirNovamente = pedido.Situacao.PermitidoPedirNovamente;

        var horarioAtualDaLocalizacaoDoPedido = await timeZoneRepository.Obter(pedido.LocalizacaoCliente.Timezone);
        var permitidoSolicitarAjuda = (!permitidoCancelar && pedidoAceito) || (pedido.Situacao.Id.Equals(situacaoRetirado.Id) && (pedido.DataHoraRetirado is null || pedido.DataHoraRetirado!.Value.Subtract(horarioAtualDaLocalizacaoDoPedido.DataHoraAtual) <= new TimeSpan(3, 0, 0)));

        return new ProjecaoDoPedidoDoCliente(
            pedido.Id,
            new(pedido.LocalizacaoCliente.Id, pedido.LocalizacaoCliente.Apelido),
            pedido.Produtos.Select(p =>
                new ProjecaoDeProdutoDoPedido(
                    p.Id,
                    p.IdProduto,
                    p.Nome,
                    p.Descricao,
                    p.Quantidade,
                    p.PrecoProduto,
                    p.PrecoUnitario,
                    p.PrecoTotal,
                    p.Imagem,
                    new ProjecaoDeSituacaoDoProduto(p.Situacao.Id, p.Situacao.Descricao),
                    new ProjecaoDeTipoDoProdutoDoPedido(p.Tipo.Id, p.Tipo.Descricao),
                    p.Adicionais?.Select(a => new ProjecaoDeAdicionalDoProdutoDoPedido(a.IdAdicional, new ProjecaoDeSituacaoDoProduto(a.Situacao.Id, a.Situacao.Descricao), a.Nome, a.PrecoUnitario, a.PrecoTotal, a.Quantidade, a.QuantidadeMaxima)))),
            pedido.Estabelecimento.AsProjecao(),
            pedido.ValorTotal,
            pedido.Observacao,
            pedido.ObservacaoDoEstabelecimento,
            permitidoCancelar,
            permitidoPedirNovamente,
            permitidoAlterarProdutos,
            permitidoAlterarHorarioDeRetirada,
            permitidoAlterarObservacao,
            permitidoSolicitarAjuda,
            pedido.DataHoraARetirar,
            pedido.Destino is not null ? new DestinoDaRetiradaDoPedidoDomain(pedido.Destino.Id, pedido.Destino.Descricao) : null);
    }

    public async Task<IEnumerable<ProjecaoDeSituacaoDoPedido>> ObterSituacoesListadasParaCliente() => (await situacaoPedidoRepository.ObterSituacoesListadasParaCliente()).Select(s => new ProjecaoDeSituacaoDoPedido(s.Id, s.Descricao, s.CorHexadecimal));

    public async Task<IEnumerable<ProjecaoDeFormaDePagamentoDoPedido>> ObterFormasDePagamentoDoPedido(Guid idPedido)
    {
        var pedido = await pedidoRepository.ObterPedido(idPedido) ?? throw new PedidoNaoEncontradoException();

        var hoje = DateOnly.FromDateTime(DateTime.Now);
        var cartoes = (await cartoesClienteRepository.ObterLista(pedido.Cliente.Id)).Where(c => c.Validade >= hoje);

        var projecaoDeFormasDePagamento = await ObterFormasComunsDePagamento();
        projecaoDeFormasDePagamento.AddRange(cartoes.Select(c => new ProjecaoDeFormaDePagamentoDoPedido(c.Id, (int)EnumTipoDeFormaDePagamento.Cartao, ObterImagem(c.Bandeira), c.Apelido, $"**** **** **** {c.FinalDoNumero}")));

        return projecaoDeFormasDePagamento;
    }

    private static string? ObterImagem(string bandeira)
    {
        //passar para banco de dados
        if (bandeira == "Mastercard")
            return "https://dashdine-estabelecimento-2.s3.amazonaws.com/formasdepagamento/master-card.png";
        if (bandeira == "Visa")
            return "https://dashdine-estabelecimento-2.s3.amazonaws.com/formasdepagamento/visa.png";
        return null;
    }

    private async Task<List<ProjecaoDeFormaDePagamentoDoPedido>> ObterFormasComunsDePagamento() =>
        await cartoesClienteRepository.UnitOfWork.TipoPagamentos
        .Where(t => t.FormaComum)
        .Select(f => new ProjecaoDeFormaDePagamentoDoPedido(null, f.Id, f.Imagem, f.Descricao, null))
        .ToListAsync();

    public async Task AtualizarPedido(UsuarioAutenticado usuarioAutenticado, Guid idPedido, DtoDoPedidoDoCliente dtoPedido)
    {
        var entidadePedido = await pedidoRepository.ObterPedido(idPedido) ?? throw new PedidoNaoEncontradoException();

        if (entidadePedido.Cliente.Id != usuarioAutenticado.Id)
            throw new PedidoNaoEncontradoException();

        if (!dtoPedido.Produtos.Any())
        {
            await pedidoRepository.CancelarPedido(entidadePedido);
            return;
        }

        var destinoDoPedido = dtoPedido.DestinoPedido != null ? DestinoDaRetiradaDoPedidoDomain.ObterPorId(dtoPedido.DestinoPedido.Id) : null;

        var produtosAtualizados = await ObterProdutosValidos(entidadePedido.Estabelecimento.Id, dtoPedido.Produtos);
        var pedidoAtualizado = entidadePedido with { Destino = destinoDoPedido, DataHoraARetirar = dtoPedido.DataHoraARetirar, Observacao = dtoPedido.Observacao, ValorTotal = ObterValorTotalPedido(produtosAtualizados), Produtos = produtosAtualizados };

        await pedidoRepository.SalvarPedido(pedidoAtualizado);
    }

    private static decimal ObterValorTotalPedido(IEnumerable<ProdutoDoPedidoDomain> produtos) => produtos.Sum(p => p.PrecoTotal).ArredondarPreco();

    public async Task SolicitarAjudaParaPedido(UsuarioAutenticado clienteAutenticado, Guid idPedido, DtoSolicitacaoDeAjuda ajuda)
    {
        var pedido = await pedidoRepository.ObterPedido(idPedido) ?? throw new PedidoNaoEncontradoException();

        if (pedido.Cliente.Id != clienteAutenticado.Id) throw new PedidoNaoEncontradoException();

        //alimentar tabela de solicitacao de ajuda por pedido no banco de dados
    }
}