using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Service.Models.Cliente.Pedido;
using Dashdine.Service.Models.Pedido;

namespace Dashdine.Service.Interface.Cliente;

public interface IPagamentoPedidoClienteService
{
    Task<ProjecaoDePagamentoDoPedidoDoCliente> PagarPedido(UsuarioAutenticado usuarioAutenticado, Guid idPedido, DtoPagarPedido pagamento);
    Task<ProjecaoDePagamentoDoPedidoDoCliente> ObterUltimoPagamentoDoPedido(UsuarioAutenticado clienteAutenticado, Guid idPedido);
    Task CancelarPagamentoExcluirPedido(UsuarioAutenticado clienteAutenticado, Guid idPedido);
    Task AtualizarPagamento(DtoPedidoPagSeguro pedidoPagSeguro);
}
