using Dashdine.Domain.Domain.Estabelecimento.HorarioDeFuncionamento;
using Dashdine.Domain.Domain.Pedido;
using Dashdine.Domain.Domain.Pedido.Cliente;
using Dashdine.Domain.Domain.Pedido.Estabelecimento;
using Dashdine.Domain.Domain.Produto;
using Dashdine.Domain.Entitys;
using Dashdine.Domain.Filtros;
using Dashdine.Domain.Interface.Pedido;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dashdine.Infrastructure.Repository.Pedido;

public sealed class PedidoRepository(IConfiguration configuration, ISituacaoPedidoRepository situacaoRepository) : BaseRepository<Domain.Entitys.Pedido>(configuration), IPedidoRepository
{
    public async Task<int> ObterQuantidadeDePedidosEmDestaque(Guid codigoCliente) =>
        await UnitOfWork.Pedidos
                .Include(p => p.IdSituacaoNavigation)
                .Where(p => p.IdCliente.Equals(codigoCliente))
                .Where(p => p.IdSituacaoNavigation.EmAndamento || p.IdSituacaoNavigation.AguardandoPagamento)
                .CountAsync();

    public async Task<PedidoDomain?> ObterPedidoEmAndamento(Guid idCliente, Guid idEstabelecimento)
    {
        return await Pedidos()
            .Where(p =>
                p.IdCliente.Equals(idCliente) &&
                p.IdEstabelecimento.Equals(idEstabelecimento) &&
                p.IdSituacaoNavigation.EmAndamento)
            .Select(p => ToDomain(p))
            .FirstOrDefaultAsync();
    }

    private Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Domain.Entitys.Pedido, ICollection<AdicionalProduto>> Pedidos()
    {
        return UnitOfWork.Pedidos
            .Include(p => p.IdLocalizacaoClienteNavigation)
            .Include(p => p.IdClienteNavigation)
            .Include(p => p.IdEstabelecimentoNavigation).ThenInclude(e => e.EnderecoEstabelecimentos)
            .Include(p => p.IdSituacaoNavigation)
            .Include(p => p.IdDestinoNavigation)
            .Include(p => p.ProdutoPedidos).ThenInclude(p => p.IdProdutoNavigation).ThenInclude(p => p.IdSituacaoNavigation)
            .Include(p => p.ProdutoPedidos).ThenInclude(p => p.IdProdutoNavigation).ThenInclude(p => p.IdTipoNavigation)
            .Include(p => p.ProdutoPedidos).ThenInclude(p => p.AdicionalProdutoPedidos).ThenInclude(a => a.IdAdicionalProdutoNavigation).ThenInclude(a => a.IdSituacaoNavigation)
            .Include(p => p.ProdutoPedidos).ThenInclude(p => p.AdicionalProdutoPedidos).ThenInclude(a => a.IdAdicionalProdutoNavigation).ThenInclude(a => a.AdicionalProdutos);
    }

    public async Task<IEnumerable<PedidoDomain>> ObterPedidosDoDia(Guid idEstabelecimento, DateOnly dia)
    {
        return await Pedidos()
            .Where(p => p.IdEstabelecimento.Equals(idEstabelecimento)
                        && p.DataHoraARetirar.HasValue
                        && p.DataHoraARetirar.Value.Date.Equals(dia)
                        && p.IdSituacaoNavigation.ListadoParaGestor)
            .Select(p => ToDomain(p))
            .ToListAsync();
    }

    public async Task<ResumoDosPedidosDoMesDomain> ObterResumoDoMesDoEstabelecimento(Guid idEstabelecimento, DateOnly mesEAno)
    {
        return await UnitOfWork.Pedidos
            .Where(p => p.DataHoraARetirar != null && p.DataHoraARetirar.Value.Month == mesEAno.Month && p.DataHoraARetirar.Value.Year == mesEAno.Year)
            .Include(p => p.IdSituacaoNavigation)
            .GroupBy(p => p.DataHoraARetirar!.Value.Month)
            .Select(pedidosDoMes =>
            new ResumoDosPedidosDoMesDomain(
                mesEAno,
                pedidosDoMes.Where(p => p.IdSituacaoNavigation.Retirado).Sum(pedido => pedido.ValorTotal),
                pedidosDoMes.Count(pedido => pedido.IdSituacaoNavigation.Retirado),
                pedidosDoMes.Count(pedido => pedido.IdSituacaoNavigation.Novo),
                pedidosDoMes.Select(pedidoDoDia =>
                    new DiaDoResumoDosPedidosDoMesDomain(
                        pedidoDoDia.DataHoraARetirar!.Value.Day,
                        (int)pedidoDoDia.DataHoraARetirar!.Value.DayOfWeek,
                        pedidosDoMes.Where(pedidos => pedidos.DataHoraARetirar!.Value.Day == pedidoDoDia.DataHoraARetirar.Value.Day && pedidos.IdSituacaoNavigation.Novo)
                        .GroupBy(pedido => pedido.DataHoraARetirar!.Value.Day)
                        .Count())
                    )))
            .FirstAsync();
    }

    private static PedidoDomain ToDomain(Domain.Entitys.Pedido pedido)
    {
        var enderecoEstabelecimento = pedido.IdEstabelecimentoNavigation.EnderecoEstabelecimentos.First();
        return new(
            pedido.Id,
            pedido.Numero,
            new ClienteDoPedidoDomain(pedido.IdCliente, pedido.IdClienteNavigation.Nome, pedido.IdClienteNavigation.Cpf, pedido.IdClienteNavigation.Email, pedido.IdClienteNavigation.Celular),
            new LocalizacaoDoClienteDomain(pedido.IdLocalizacaoCliente, pedido.IdLocalizacaoClienteNavigation.Apelido, pedido.IdLocalizacaoClienteNavigation.Latitude, pedido.IdLocalizacaoClienteNavigation.Longitude, pedido.IdLocalizacaoClienteNavigation.Timezone),
            new EstabelecimentoDoPedidoDoClienteDomain(pedido.IdEstabelecimento, pedido.IdEstabelecimentoNavigation.Logo, pedido.IdEstabelecimentoNavigation.NomeFantasia,
                new Domain.Domain.Pedido.Cliente.EnderecoEstabelecimento(
                    enderecoEstabelecimento.Id,
                    enderecoEstabelecimento.Cep,
                    enderecoEstabelecimento.Logradouro,
                    enderecoEstabelecimento.Numero,
                    enderecoEstabelecimento.Complemento,
                    enderecoEstabelecimento.Bairro,
                    enderecoEstabelecimento.Cidade,
                    enderecoEstabelecimento.Estado,
                    enderecoEstabelecimento.Latitude,
                    enderecoEstabelecimento.Longitude,
                    enderecoEstabelecimento.Timezone
                    )),
            SituacaoToDomain(pedido.IdSituacaoNavigation),
            pedido.IdDestinoNavigation != null ? new DestinoDaRetiradaDoPedidoDomain(pedido.IdDestino!.Value, pedido.IdDestinoNavigation.Descricao) : null,
            pedido.DataHora,
            pedido.DataHoraARetirar,
            pedido.Observacao,
            pedido.ObservacaoEstabelecimento,
            pedido.ValorTotal,
            pedido.DataHoraRetirado,
            ProdutosToDomain(pedido.ProdutoPedidos)
        );
    }

    private static SituacaoDoPedidoDomain SituacaoToDomain(SituacaoPedido situacao) =>
        new(
            situacao.Id,
            situacao.Descricao,
            situacao.ListadoParaCliente,
            situacao.ListadoParaGestor,
            situacao.PossuiCategoriaPropriaParaCliente,
            situacao.PermitidoCancelar,
            situacao.IntervaloMinimoEmHorasAntesDeRetirarParaCancelar,
            situacao.ApagarPedidoAoCancelar,
            situacao.PermitidoSolicitarAjuda,
            situacao.PermitidoRetirar,
            situacao.PermitidoPedirNovamente,
            situacao.VisualizaMotivo,
            situacao.CorHexadecimal,
            situacao.PermitidoRejeitar,
            situacao.PermitidoPagar,
            situacao.EstornarPagamentoAoCancelar);

    private static IEnumerable<ProdutoDoPedidoDomain> ProdutosToDomain(ICollection<ProdutoPedido> produtoPedidos) =>
        produtoPedidos
                .Select(p =>
                    new ProdutoDoPedidoDomain(
                        p.Id,
                        p.IdProduto,
                        new SituacaoDeProdutoDomain(p.IdProdutoNavigation.IdSituacaoNavigation.Id, p.IdProdutoNavigation.IdSituacaoNavigation.Descricao),
                        new TipoDoProdutoDomain(p.IdProdutoNavigation.IdTipoNavigation.Id, p.IdProdutoNavigation.IdTipoNavigation.Descricao),
                        p.IdProdutoNavigation.Nome,
                        p.IdProdutoNavigation.Descricao,
                        p.IdProdutoNavigation.Imagem,
                        p.Quantidade,
                        p.PrecoProduto,
                        p.PrecoUnitario,
                        p.PrecoTotal,
                        p.AdicionalProdutoPedidos
                            .Select(a =>
                                new AdicionalDoProdutoDoPedidoDomain(
                                    a.IdAdicionalProduto,
                                    a.IdProdutoPedido,
                                    new SituacaoDeProdutoDomain(a.IdAdicionalProdutoNavigation.IdSituacaoNavigation.Id, a.IdAdicionalProdutoNavigation.IdSituacaoNavigation.Descricao),
                                    a.IdAdicionalProdutoNavigation.Nome,
                                    a.Quantidade,
                                    a.IdAdicionalProdutoNavigation.Preco,
                                    a.IdAdicionalProdutoNavigation.AdicionalProdutos.Where(adicional => adicional.IdProduto == p.IdProduto && adicional.IdAdicional == a.IdAdicionalProduto).Select(a => a.QtdeMaxima).First())).ToList()));

    public async Task<IEnumerable<PedidoDomain>> ObterPedidosDoCliente(FiltrosDomain filtros, Guid codigoCliente, IEnumerable<SituacaoDoPedidoDomain> situacoes, bool apenasPedidosRetirados = false)
    {
        string filtroListadoParaCliente = apenasPedidosRetirados ? "AND SP.RETIRADO = TRUE" : "AND SP.LISTADO_PARA_CLIENTE = TRUE";
        string whereSituacoesIn = situacoes.Any() && !apenasPedidosRetirados ? $" AND SP.ID IN ({string.Join(", ", situacoes.Select(s => s.Id).ToList())})" : string.Empty;
        string limitOffset = filtros.QuantidadeDeRegistros > 0 && filtros.Pagina > 0 ? $"LIMIT {filtros.QuantidadeDeRegistros} OFFSET {(filtros.Pagina - 1) * filtros.QuantidadeDeRegistros}" : string.Empty;

        return await UnitOfWork.PedidosDoClienteView
            .FromSqlRaw(@$"SELECT 
								P.ID, 
                                P.NUMERO,
								P.ID_CLIENTE, 
								P.DATA_HORA, 
								P.DATA_HORA_A_RETIRAR, 
								P.OBSERVACAO, 
								P.OBSERVACAO_ESTABELECIMENTO, 
								P.VALOR_TOTAL, 
								P.ID_SITUACAO ID_SITUACAO_PEDIDO,
								P.DATA_HORA_RETIRADO, 

                                C.NOME NOME_CLIENTE,
                                C.EMAIL EMAIL_CLIENTE,
                                C.CELULAR CELULAR_CLIENTE,
                                C.CPF CPF_CLIENTE,
                                
                                SP.DESCRICAO DESCRICAO_SITUACAO_PEDIDO,                                
								SP.LISTADO_PARA_CLIENTE SITUACAO_LISTADA_PARA_CLIENTE,
								SP.LISTADO_PARA_GESTOR SITUACAO_LISTADA_PARA_GESTOR,
								SP.POSSUI_CATEGORIA_PROPRIA_PARA_CLIENTE SITUACAO_POSSUI_CATEGORIA_PROPRIA,
								SP.PERMITIDO_CANCELAR SITUACAO_PERMITIDO_CANCELAR,
								SP.INTERVALO_MINIMO_EM_HORAS_ANTES_DE_RETIRAR_PARA_CANCELAR SITUACAO_INTERVALO_MINIMO_EM_HORAS,
								SP.APAGAR_PEDIDO_AO_CANCELAR SITUACAO_APAGAR_PEDIDO_AO_CANCELAR,
								SP.PERMITIDO_SOLICITAR_AJUDA SITUACAO_PERMITIDO_SOLICITAR_AJUDA,
								SP.PERMITIDO_RETIRAR SITUACAO_PERMITIDO_RETIRAR,
								SP.PERMITIDO_PEDIR_NOVAMENTE SITUACAO_PERMITIDO_PEDIR_NOVAMENTE,
								SP.VISUALIZA_MOTIVO SITUACAO_VISUALIZA_MOTIVO,
                                SP.COR_HEXADECIMAL SITUACAO_COR_HEXADECIMAL,
								SP.PERMITIDO_REJEITAR SITUACAO_PERMITIDO_REJEITAR,
								SP.PERMITIDO_PAGAR SITUACAO_PERMITIDO_PAGAR,
								SP.ESTORNAR_PAGAMENTO_AO_CANCELAR SITUACAO_ESTORNAR_PAGAMENTO_AO_CANCELAR,
								
                                P.ID_DESTINO ID_DESTINO_PEDIDO, 
								DP.DESCRICAO DESCRICAO_DESTINO,
								
                                P.ID_LOCALIZACAO_CLIENTE, 
								EC.APELIDO APELIDO_LOCALIZACAO_CLIENTE, 
								EC.LATITUDE LATITUDE_LOCALIZACAO_CLIENTE, 
								EC.LONGITUDE LONGITUDE_LOCALIZACAO_CLIENTE, 
                                EC.TIMEZONE TIMEZONE_LOCALIZACAO_CLIENTE,
								
                                P.ID_ESTABELECIMENTO, 
								E.LOGO LOGO_ESTABELECIMENTO, 
								E.NOME_FANTASIA NOME_ESTABELECIMENTO, 
								EE.ID ID_ENDERECO_ESTABELECIMENTO, 
								EE.CEP CEP_ENDERECO_ESTABELECIMENTO, 
								EE.LOGRADOURO LOGRADOURO_ENDERECO_ESTABELECIMENTO, 
								EE.NUMERO NUMERO_ENDERECO_ESTABELECIMENTO, 
								EE.COMPLEMENTO COMPLEMENTO_ENDERECO_ESTABELECIMENTO, 
								EE.BAIRRO BAIRRO_ENDERECO_ESTABELECIMENTO, 
								EE.CIDADE CIDADE_ENDERECO_ESTABELECIMENTO, 
								EE.ESTADO ESTADO_ENDERECO_ESTABELECIMENTO, 
								EE.LATITUDE LATITUDE_ENDERECO_ESTABELECIMENTO, 
								EE.LONGITUDE LONGITUDE_ENDERECO_ESTABELECIMENTO,
								EE.TIMEZONE TIMEZONE_LOCALIZACAO_ESTABELECIMENTO
						   FROM PEDIDO P 
                           INNER JOIN CLIENTE C ON C.ID = P.ID_CLIENTE
						   INNER JOIN ENDERECO_CLIENTE EC ON EC.ID = P.ID_LOCALIZACAO_CLIENTE 
						   INNER JOIN ESTABELECIMENTO E ON E.ID = P.ID_ESTABELECIMENTO 
						   INNER JOIN ENDERECO_ESTABELECIMENTO EE ON EE.ID_ESTABELECIMENTO = E.ID
						   INNER JOIN SITUACAO_PEDIDO SP ON SP.ID  = P.ID_SITUACAO {filtroListadoParaCliente}
						   LEFT JOIN DESTINO_PEDIDO DP ON DP.ID = P.ID_DESTINO 
						   WHERE 
						   		(P.DATA_HORA_A_RETIRAR >= (TIMEZONE('UTC', NOW()) - INTERVAL '48 HOURS') OR P.DATA_HORA_A_RETIRAR IS NULL) AND
						   		P.ID_CLIENTE = '{codigoCliente}' {whereSituacoesIn} 
						   ORDER BY SP.POSSUI_CATEGORIA_PROPRIA_PARA_CLIENTE DESC, SP.ID, P.DATA_HORA_A_RETIRAR
                           {limitOffset}")
            .Select(pedidoView => new PedidoDomain(
                                        pedidoView.Id,
                                        pedidoView.Numero,
                                        new ClienteDoPedidoDomain(pedidoView.IdCliente, pedidoView.NomeCliente, pedidoView.CpfCliente, pedidoView.EmailCliente, pedidoView.CelularCliente),
                                        new(pedidoView.IdLocalizacaoCliente, pedidoView.ApelidoLocalizacaoCliente, pedidoView.LatitudeLocalizacaoCliente, pedidoView.LongitudeLocalizacaoCliente, pedidoView.TimeZoneLocalizacaoCliente),
                                        new(pedidoView.IdEstabelecimento, pedidoView.LogoEstabelecimento ?? string.Empty, pedidoView.NomeEstabelecimento,
                                            new Domain.Domain.Pedido.Cliente.EnderecoEstabelecimento(
                                                pedidoView.IdEnderecoEstabelecimento,
                                                pedidoView.CepEnderecoEstabelecimento,
                                                pedidoView.LogradouroEnderecoEstabelecimento,
                                                pedidoView.NumeroEnderecoEstabelecimento,
                                                pedidoView.ComplementoEnderecoEstabelecimento,
                                                pedidoView.BairroEnderecoEstabelecimento,
                                                pedidoView.CidadeEnderecoEstabelecimento,
                                                pedidoView.EstadoEnderecoEstabelecimento,
                                                pedidoView.LatitudeEnderecoEstabelecimento,
                                                pedidoView.LongitudeEnderecoEstabelecimento,
                                                pedidoView.TimeZoneLocalizacaoEstabelecimento
                                                )),
                                        new SituacaoDoPedidoDomain(pedidoView.IdSituacaoPedido, pedidoView.DescricaoSituacaoPedido, pedidoView.SituacaoListadaParaCliente, pedidoView.SituacaoListadaParaGestor, pedidoView.SituacaoPossuiCategoriaPropria, pedidoView.SituacaoPermitidoCancelar, pedidoView.SituacaoIntervaloMinimoEmHorasAntesDeRetirarParaCancelar, pedidoView.SituacaoApagarPedidoAoCancelar, pedidoView.SituacaoPermitidoSolicitarAjuda, pedidoView.SituacaoPermitidoRetirar, pedidoView.SituacaoPermitidoPedirNovamente, pedidoView.SituacaoVisualizaMotivo, pedidoView.SituacaoCorHexadecimal, pedidoView.SituacaoPermitidoRejeitar, pedidoView.SituacaoPermitidoPagar, pedidoView.SituacaoEstornarPagamentoAoCancelar),
                                        pedidoView.IdDestinoPedido != null ? new(pedidoView.IdDestinoPedido.Value, pedidoView.DescricaoDestino!) : null,
                                        pedidoView.DataHora,
                                        pedidoView.DataHoraARetirar,
                                        pedidoView.Observacao,
                                        pedidoView.ObservacaoEstabelecimento,
                                        pedidoView.ValorTotal,
                                        pedidoView.DataHoraRetirado,
                                        ProdutosToDomain(
                                            UnitOfWork.ProdutoPedidos
                                            .Include(p => p.IdProdutoNavigation).ThenInclude(p => p.IdSituacaoNavigation)
                                            .Include(p => p.IdProdutoNavigation).ThenInclude(p => p.IdTipoNavigation)
                                            .Include(p => p.AdicionalProdutoPedidos).ThenInclude(a => a.IdAdicionalProdutoNavigation).ThenInclude(a => a.IdSituacaoNavigation)
                                            .Include(p => p.AdicionalProdutoPedidos).ThenInclude(a => a.IdAdicionalProdutoNavigation).ThenInclude(a => a.AdicionalProdutos)
                                            .AsQueryable()
                                            .Where(p => p.IdPedido.Equals(pedidoView.Id))
                                            .ToList())))
            .ToListAsync();
    }

    public async Task SalvarCabecalhoPedido(PedidoDomain pedidoAtualizado)
    {
        await AtualizarCabecalho(pedidoAtualizado);
        await SaveChangesAsync();
    }

    private async Task<Domain.Entitys.Pedido> AtualizarCabecalho(PedidoDomain pedidoAtualizado)
    {
        var pedido = await UnitOfWork.Pedidos
            .Include(p => p.ProdutoPedidos)
            .ThenInclude(p => p.AdicionalProdutoPedidos)
            .FirstOrDefaultAsync(p => p.Id == pedidoAtualizado.Id);

        if (pedido is null)
        {
            pedido = new Domain.Entitys.Pedido()
            {
                Id = pedidoAtualizado.Id,
                IdCliente = pedidoAtualizado.Cliente.Id,
                IdEstabelecimento = pedidoAtualizado.Estabelecimento.Id,
                IdLocalizacaoCliente = pedidoAtualizado.LocalizacaoCliente.Id,
                DataHora = pedidoAtualizado.DataHora
            };
            UnitOfWork.Pedidos.Add(pedido);
        }

        pedido.IdSituacao = pedidoAtualizado.Situacao.Id;
        pedido.DataHoraARetirar = pedidoAtualizado.DataHoraARetirar;
        pedido.DataHoraRetirado = pedidoAtualizado.DataHoraRetirado;
        pedido.IdDestino = pedidoAtualizado.Destino?.Id ?? null;
        pedido.Observacao = pedidoAtualizado.Observacao;
        pedido.ObservacaoEstabelecimento = pedidoAtualizado.ObservacaoDoEstabelecimento;
        pedido.ValorTotal = pedidoAtualizado.ValorTotal;

        return pedido;
    }

    public async Task SalvarPedido(PedidoDomain pedidoAtualizado)
    {
        var pedido = await AtualizarCabecalho(pedidoAtualizado);

        pedido.ProdutoPedidos
            .Select(a => a.AdicionalProdutoPedidos)
            .ToList()
            .ForEach(a => UnitOfWork.AdicionalProdutoPedidos.RemoveRange(a));
        UnitOfWork.ProdutoPedidos.RemoveRange(pedido.ProdutoPedidos);

        foreach (var produtoDoPedidoAtualizado in pedidoAtualizado.Produtos)
        {
            var entidadeProduto = new ProdutoPedido()
            {
                Id = produtoDoPedidoAtualizado.Id,
                IdPedido = pedidoAtualizado.Id,
                IdProduto = produtoDoPedidoAtualizado.IdProduto,
                PrecoUnitario = produtoDoPedidoAtualizado.PrecoUnitario,
                PrecoTotal = produtoDoPedidoAtualizado.PrecoTotal,
                PrecoProduto = produtoDoPedidoAtualizado.PrecoProduto,
                Quantidade = produtoDoPedidoAtualizado.Quantidade
            };

            pedido.ProdutoPedidos.Add(entidadeProduto);

            if (produtoDoPedidoAtualizado.Adicionais?.Any() ?? false)
            {
                foreach (var adicional in produtoDoPedidoAtualizado.Adicionais)
                {
                    var entidadeAdicional = new AdicionalProdutoPedido()
                    {
                        IdProdutoPedido = produtoDoPedidoAtualizado.Id,
                        IdAdicionalProduto = adicional.IdAdicional,
                        Quantidade = adicional.Quantidade,
                        PrecoUnitario = adicional.PrecoUnitario,
                        PrecoTotal = adicional.PrecoTotal
                    };
                    entidadeProduto.AdicionalProdutoPedidos.Add(entidadeAdicional);
                }
            }
        }

        await SaveChangesAsync();
    }

    public async Task<PedidoDomain?> ObterPedido(Guid idPedido)
        => await Pedidos()
            .Where(p => p.Id.Equals(idPedido))
            .Select(p => ToDomain(p))
            .FirstOrDefaultAsync();

    public async Task CancelarPedido(PedidoDomain pedido)
    {
        if (!pedido.Situacao.ApagarPedidoAoCancelar)
        {
            await CancelarPedido(pedido.Id);
            return;
        }

        var entidade = await UnitOfWork.Pedidos.Include(p => p.ProdutoPedidos).ThenInclude(p => p.AdicionalProdutoPedidos).Where(p => p.Id.Equals(pedido.Id)).FirstOrDefaultAsync();
        if (entidade is null) return;

        entidade.ProdutoPedidos.ToList().ForEach(p => UnitOfWork.AdicionalProdutoPedidos.RemoveRange(p.AdicionalProdutoPedidos));
        await SaveChangesAsync();

        UnitOfWork.ProdutoPedidos.RemoveRange(entidade.ProdutoPedidos);
        await SaveChangesAsync();

        UnitOfWork.Pedidos.Remove(entidade);
        await SaveChangesAsync();
    }

    private async Task CancelarPedido(Guid idPedido)
    {
        var entidade = await UnitOfWork.Pedidos.Where(p => p.Id.Equals(idPedido)).FirstOrDefaultAsync();
        if (entidade is null) return;
        entidade.IdSituacao = (await situacaoRepository.ObterSituacaoCancelado()).Id;
        UnitOfWork.Pedidos.Update(entidade);
        await SaveChangesAsync();
    }
}
