using Dashdine.Domain.Domain.Pedido;
using Dashdine.Domain.Interface.Pedido;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dashdine.Infrastructure.Repository.Pedido;

public class SituacaoPedidoRepository(IConfiguration configuration) : BaseRepository<Domain.Entitys.SituacaoPedido>(configuration), ISituacaoPedidoRepository
{
    public async Task<SituacaoDoPedidoDomain> ObterSituacaoEmAndamento() => await UnitOfWork.SituacaoPedidos.AsNoTracking().AsQueryable().Where(s => s.EmAndamento).Select(s => ToDomain(s)).FirstAsync();
    public async Task<SituacaoDoPedidoDomain> ObterSituacaoAceito() => await UnitOfWork.SituacaoPedidos.AsNoTracking().AsQueryable().Where(s => s.Aceito).Select(s => ToDomain(s)).FirstAsync();
    public async Task<SituacaoDoPedidoDomain> ObterSituacaoAguardandoPagamento() => await UnitOfWork.SituacaoPedidos.AsNoTracking().AsQueryable().Where(s => s.AguardandoPagamento).Select(s => ToDomain(s)).FirstAsync();
    public async Task<SituacaoDoPedidoDomain> ObterSituacaoRetirado() => await UnitOfWork.SituacaoPedidos.AsNoTracking().AsQueryable().Where(s => s.Retirado).Select(s => ToDomain(s)).FirstAsync();
    public async Task<SituacaoDoPedidoDomain> ObterSituacaoRejeitado() => await UnitOfWork.SituacaoPedidos.AsNoTracking().AsQueryable().Where(s => s.Rejeitado).Select(s => ToDomain(s)).FirstAsync();
    public async Task<SituacaoDoPedidoDomain> ObterSituacaoNovo() => await UnitOfWork.SituacaoPedidos.AsNoTracking().AsQueryable().Where(s => s.Novo).Select(s => ToDomain(s)).FirstAsync();

    public async Task<IEnumerable<SituacaoDoPedidoDomain>> ObterSituacoes() => await UnitOfWork.SituacaoPedidos.Select(s => ToDomain(s)).ToListAsync();
    public async Task<IEnumerable<SituacaoDoPedidoDomain>> ObterSituacoesListadasParaCliente() => await UnitOfWork.SituacaoPedidos.Where(s => s.ListadoParaCliente).Select(s => ToDomain(s)).ToListAsync();

    private static SituacaoDoPedidoDomain ToDomain(Domain.Entitys.SituacaoPedido entity) => new(entity.Id, entity.Descricao, entity.ListadoParaCliente, entity.ListadoParaGestor, entity.PossuiCategoriaPropriaParaCliente, entity.PermitidoCancelar, entity.IntervaloMinimoEmHorasAntesDeRetirarParaCancelar, entity.ApagarPedidoAoCancelar, entity.PermitidoSolicitarAjuda, entity.PermitidoRetirar, entity.PermitidoPedirNovamente, entity.VisualizaMotivo, entity.CorHexadecimal, entity.PermitidoRejeitar, entity.PermitidoPagar, entity.EstornarPagamentoAoCancelar);

    public async Task<SituacaoDoPedidoDomain> ObterSituacaoCancelado() => await UnitOfWork.SituacaoPedidos.AsNoTracking().AsQueryable().Where(s => s.Cancelado).Select(s => ToDomain(s)).FirstAsync();
}
