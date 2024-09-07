using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Service.Models.Estabelecimento.Pedido;

namespace Dashdine.Service.Interface.Estabelecimento;

public interface IPedidoEstabelecimentoService
{
    Task<ProjecaoDeResumoDosPedidosDoMes> ObterResumoDoMes(UsuarioAutenticado usuario, Guid idEstabelecimento, DateTime? mes = null);
    Task<IEnumerable<ProjecaoDePedidoDoDia>> ObterPedidosDoDia(UsuarioAutenticado usuario, Guid idEstabelecimento, DateOnly data);
    Task<ProjecaoDePedidoDoDia> AceitarPedido(UsuarioAutenticado usuario, Guid idEstabelecimento, Guid idPedido);
    Task<ProjecaoDePedidoDoDia> EntregarPedido(UsuarioAutenticado usuario, Guid idEstabelecimento, Guid idPedido);
    Task<ProjecaoDePedidoDoDia> RejeitarPedido(UsuarioAutenticado usuario, Guid idEstabelecimento, Guid idPedido, DtoRejeitarPedido rejeitarPedido);
    Task<ProjecaoDePedidoDoDia> DesfazerEntregaDoPedido(UsuarioAutenticado usuario, Guid idEstabelecimento, Guid idPedido);
}
