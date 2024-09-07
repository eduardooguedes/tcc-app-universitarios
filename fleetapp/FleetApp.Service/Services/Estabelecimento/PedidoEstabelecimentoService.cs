using Dashdine.Domain.Domain.Pedido;
using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Domain.Entitys.Views;
using Dashdine.Domain.Interface;
using Dashdine.Domain.Interface.Estabelecimento;
using Dashdine.Domain.Interface.Pedido;
using Dashdine.Service.Exceptions.Cliente.Pedido;
using Dashdine.Service.Exceptions.Estabelecimento;
using Dashdine.Service.Extensions;
using Dashdine.Service.Interface.Estabelecimento;
using Dashdine.Service.Models.Estabelecimento.Pedido;

namespace Dashdine.Service.Services.Estabelecimento;

public sealed class PedidoEstabelecimentoService(IPedidoRepository pedidoRepository, IGestorRepository gestorRepository, ISituacaoPedidoRepository situacaoRepository, ITimeZoneRepository timeZoneRepository, IEstabelecimentoRepository estabelecimentoRepository) : IPedidoEstabelecimentoService
{
    private async Task ValidarSeEstabelecimentoDoGestor(Guid idGestor, Guid idEstabelecimento)
    {
        var estabelecimentos = await gestorRepository.ObterListaDeEstabelecimentosDoGestor(idGestor)
            ?? throw new EstabelecimentoNaoEncontradoException();

        if (!estabelecimentos.Any(e => e.Id.Equals(idEstabelecimento)))
            throw new EstabelecimentoNaoEncontradoException();
    }

    public async Task<IEnumerable<ProjecaoDePedidoDoDia>> ObterPedidosDoDia(UsuarioAutenticado usuario, Guid idEstabelecimento, DateOnly data)
    {
        await ValidarSeEstabelecimentoDoGestor(usuario.Id, idEstabelecimento);
        var pedidosDoDia = await pedidoRepository.ObterPedidosDoDia(idEstabelecimento, data);

        var pedidos = new List<ProjecaoDePedidoDoDia>();
        if (!pedidosDoDia.Any()) return pedidos;

        var situacaoAceito = await situacaoRepository.ObterSituacaoAceito();
        var situacaoRetirado = await situacaoRepository.ObterSituacaoRetirado();
        var situacaoNovo = await situacaoRepository.ObterSituacaoNovo();

        var hoje = await ObterHorarioAtualDoEstabelecimento(idEstabelecimento);

        pedidosDoDia.ToList().ForEach(pedidoDoDia => pedidos.Add(DomainAsProjecao(pedidoDoDia, situacaoNovo, situacaoAceito, situacaoRetirado, hoje)));
        return pedidos;
    }

    private static ProjecaoDePedidoDoDia DomainAsProjecao(PedidoDomain pedidoDoDia, SituacaoDoPedidoDomain situacaoNovo, SituacaoDoPedidoDomain situacaoAceito, SituacaoDoPedidoDomain situacaoRetirado, Timezone hoje)
    {
        var hojeDiaDaRetirada = pedidoDoDia.DataHoraARetirar!.Value.Date.Equals(hoje.DataHoraAtual.Date);
        return new ProjecaoDePedidoDoDia(
                    pedidoDoDia.Id,
                    pedidoDoDia.Numero,
                    pedidoDoDia.DataHoraARetirar.ToTimeOnly()!.Value,
                    pedidoDoDia.Situacao.AsProjecao(),
                    new ProjecaoDeClienteParaPedidoDoDia(pedidoDoDia.Cliente.Id, pedidoDoDia.Cliente.Nome, pedidoDoDia.Cliente.Email, pedidoDoDia.Cliente.Celular),
                    pedidoDoDia.ValorTotal,
                    pedidoDoDia.Produtos.Select(produto => produto.AsProjecaoParaListagem()),
                    pedidoDoDia.Observacao,
                    pedidoDoDia.ObservacaoDoEstabelecimento,
                    PermitidoAceitarPedido(pedidoDoDia.Situacao, situacaoNovo, DateOnly.FromDateTime(pedidoDoDia.DataHoraARetirar.Value), DateOnly.FromDateTime(hoje.DataHoraAtual)),
                    PermitidoRejeitarPedido(pedidoDoDia.Situacao),
                    PermitidoEntregarPedido(pedidoDoDia.Situacao, situacaoAceito, hojeDiaDaRetirada),
                    PermitidoDesfazerEntregaDoPedido(pedidoDoDia.Situacao, situacaoRetirado, hojeDiaDaRetirada));
    }

    private async Task<Timezone> ObterHorarioAtualDoEstabelecimento(Guid idEstabelecimento) => await timeZoneRepository.Obter((await estabelecimentoRepository.ObterEstabelecimentoDoCliente(idEstabelecimento))?.Endereco.TimeZone);

    private static bool PermitidoRejeitarPedido(SituacaoDoPedidoDomain situacaoDoPedido) => situacaoDoPedido.PermitidoRejeitar;

    private static bool PermitidoAceitarPedido(SituacaoDoPedidoDomain situacao, SituacaoDoPedidoDomain situacaoNovo, DateOnly dataHoraARetirar, DateOnly hoje) => situacao.Id.Equals(situacaoNovo.Id) && dataHoraARetirar >= hoje;

    private static bool PermitidoEntregarPedido(SituacaoDoPedidoDomain situacaoDoPedido, SituacaoDoPedidoDomain situacaoAceito, bool hojeDiaDaRetirada) => situacaoDoPedido.Id.Equals(situacaoAceito.Id) && hojeDiaDaRetirada;

    private static bool PermitidoDesfazerEntregaDoPedido(SituacaoDoPedidoDomain situacaoDoPedido, SituacaoDoPedidoDomain situacaoRetirado, bool hojeDiaDaRetirada) => situacaoDoPedido.Id.Equals(situacaoRetirado.Id) && hojeDiaDaRetirada;

    public async Task<ProjecaoDeResumoDosPedidosDoMes> ObterResumoDoMes(UsuarioAutenticado usuario, Guid idEstabelecimento, DateTime? mes = null)
    {
        var mesDoResumo = mes ?? DateTime.Now;

        await ValidarSeEstabelecimentoDoGestor(usuario.Id, idEstabelecimento);

        var resumo = await pedidoRepository
            .ObterResumoDoMesDoEstabelecimento(idEstabelecimento, DateOnly.FromDateTime(mesDoResumo));

        return new ProjecaoDeResumoDosPedidosDoMes(resumo.MesEAno, resumo.TotalVendido, resumo.QuantidadeDePedidosRetirados, resumo.QuantidadeDePedidosNovos, resumo.Dias.Select(d => new ProjecaoDeDiaDoResumoDosPedidosDoMes(d.DiaDoMes, d.DiaDaSemana, d.QuantidadeDePedidosNovos)));
    }

    private async Task<PedidoDomain> ObterPedidoValido(Guid idPedido, Guid idEstabelecimento)
    {
        var pedido = await pedidoRepository.ObterPedido(idPedido)
            ?? throw new PedidoNaoEncontradoException();

        if (!pedido.Estabelecimento.Id.Equals(idEstabelecimento))
            throw new PedidoNaoEncontradoException();

        if (!pedido.DataHoraARetirar.HasValue)
            throw new PedidoNaoEncontradoException();

        if (!pedido.Situacao.ListadoParaGestor)
            throw new PedidoNaoEncontradoException();

        return pedido;
    }

    public async Task<ProjecaoDePedidoDoDia> AceitarPedido(UsuarioAutenticado usuario, Guid idEstabelecimento, Guid idPedido)
    {
        await ValidarSeEstabelecimentoDoGestor(usuario.Id, idEstabelecimento);
        var pedido = await ObterPedidoValido(idPedido, idEstabelecimento);
        var situacaoNovo = await situacaoRepository.ObterSituacaoNovo();
        var hoje = await ObterHorarioAtualDoEstabelecimento(idEstabelecimento);

        if (!PermitidoAceitarPedido(pedido.Situacao, situacaoNovo, DateOnly.FromDateTime(pedido.DataHoraARetirar!.Value), DateOnly.FromDateTime(hoje.DataHoraAtual)))
            throw new NaoEhPossivelAceitarPedidoException();

        var situacaoAceito = await situacaoRepository.ObterSituacaoAceito();
        var pedidoAceito = pedido with { Situacao = situacaoAceito };

        await pedidoRepository.SalvarCabecalhoPedido(pedidoAceito);
        return DomainAsProjecao(pedidoAceito, situacaoNovo, situacaoAceito, await situacaoRepository.ObterSituacaoRetirado(), hoje);
    }

    public async Task<ProjecaoDePedidoDoDia> EntregarPedido(UsuarioAutenticado usuario, Guid idEstabelecimento, Guid idPedido)
    {
        await ValidarSeEstabelecimentoDoGestor(usuario.Id, idEstabelecimento);
        var pedido = await ObterPedidoValido(idPedido, idEstabelecimento);
        var situacaoAceito = await situacaoRepository.ObterSituacaoAceito();

        var hoje = await ObterHorarioAtualDoEstabelecimento(idEstabelecimento);
        var hojeDiaDaRetirada = pedido.DataHoraARetirar!.Value.Date.Equals(hoje.DataHoraAtual.Date);

        if (!PermitidoEntregarPedido(pedido.Situacao, situacaoAceito, hojeDiaDaRetirada))
            throw new NaoEhPossivelEntregarPedidoException();

        var situacaoRetirado = await situacaoRepository.ObterSituacaoRetirado();
        var pedidoRetirado = pedido with { Situacao = situacaoRetirado, DataHoraRetirado = hoje.DataHoraAtual };

        await pedidoRepository.SalvarCabecalhoPedido(pedidoRetirado);
        return DomainAsProjecao(pedidoRetirado, await situacaoRepository.ObterSituacaoNovo(), situacaoAceito, situacaoRetirado, hoje);
    }

    public async Task<ProjecaoDePedidoDoDia> RejeitarPedido(UsuarioAutenticado usuario, Guid idEstabelecimento, Guid idPedido, DtoRejeitarPedido rejeitarPedido)
    {
        await ValidarSeEstabelecimentoDoGestor(usuario.Id, idEstabelecimento);
        var pedido = await ObterPedidoValido(idPedido, idEstabelecimento);

        if (!PermitidoRejeitarPedido(pedido.Situacao))
            throw new NaoEhPossivelRejeitarPedidoException();

        var situacaoRejeitado = await situacaoRepository.ObterSituacaoRejeitado();
        var pedidoRejeitado = pedido with { Situacao = situacaoRejeitado, ObservacaoDoEstabelecimento = rejeitarPedido.Motivo };

        await pedidoRepository.SalvarCabecalhoPedido(pedidoRejeitado);
        return DomainAsProjecao(pedidoRejeitado, await situacaoRepository.ObterSituacaoNovo(), await situacaoRepository.ObterSituacaoAceito(), await situacaoRepository.ObterSituacaoRetirado(), await ObterHorarioAtualDoEstabelecimento(idEstabelecimento));
    }

    public async Task<ProjecaoDePedidoDoDia> DesfazerEntregaDoPedido(UsuarioAutenticado usuario, Guid idEstabelecimento, Guid idPedido)
    {
        await ValidarSeEstabelecimentoDoGestor(usuario.Id, idEstabelecimento);
        var pedido = await ObterPedidoValido(idPedido, idEstabelecimento);
        var situacaoRetirado = await situacaoRepository.ObterSituacaoRetirado();
        var hoje = await ObterHorarioAtualDoEstabelecimento(idEstabelecimento);
        var hojeDiaDaRetirada = pedido.DataHoraARetirar!.Value.Date.Equals(hoje.DataHoraAtual.Date);

        var permitidoDesfazerEntrega = PermitidoDesfazerEntregaDoPedido(pedido.Situacao, situacaoRetirado, hojeDiaDaRetirada);
        if (!permitidoDesfazerEntrega)
            throw new NaoEhPossivelDesfazerEntregaDoPedidoException();

        var situacaoAceito = await situacaoRepository.ObterSituacaoAceito();
        var pedidoAceito = pedido with { Situacao = situacaoAceito, DataHoraRetirado = null };

        await pedidoRepository.SalvarCabecalhoPedido(pedidoAceito);
        return DomainAsProjecao(pedidoAceito, await situacaoRepository.ObterSituacaoNovo(), situacaoAceito, situacaoRetirado, hoje);
    }
}
