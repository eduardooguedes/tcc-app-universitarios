using Dashdine.Domain.Domain.Pedido;
using Dashdine.Domain.Domain.Pedido.Estabelecimento;
using Dashdine.Domain.Filtros;

namespace Dashdine.Domain.Interface.Pedido;

public interface IPedidoRepository
{
    Task<int> ObterQuantidadeDePedidosEmDestaque(Guid codigoCliente);
    Task<IEnumerable<PedidoDomain>> ObterPedidosDoCliente(FiltrosDomain filtros, Guid codigoCliente, IEnumerable<SituacaoDoPedidoDomain> situacoes, bool apenasPedidosRetirados = false);
    Task<PedidoDomain?> ObterPedido(Guid idPedido);
    Task<PedidoDomain?> ObterPedidoEmAndamento(Guid idCliente, Guid idEstabelecimento);
    Task<ResumoDosPedidosDoMesDomain> ObterResumoDoMesDoEstabelecimento(Guid idEstabelecimento, DateOnly mes);
    Task<IEnumerable<PedidoDomain>> ObterPedidosDoDia(Guid idEstabelecimento, DateOnly dia);
    Task SalvarCabecalhoPedido(PedidoDomain pedidoAtualizado);
    Task SalvarPedido(PedidoDomain pedido);
    Task CancelarPedido(PedidoDomain pedido);
}
