using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Service.Models.Cliente.Estabelecimento;
using Dashdine.Service.Models.Cliente.Pedido;
using Dashdine.Service.Models.Filtros;

namespace Dashdine.Service.Interface.Cliente;

public interface IPedidoClienteService
{
    Task<ProjecaoDeInformacaoSobreOsPedidosDoCliente> ObterInformacoesSobreOsPedidosDoCliente(Guid idCliente);
    Task<IEnumerable<ProjecaoDeCategoriaParaPedidoDoClienteParaListagem>> ObterPedidosDoCliente(FiltrosRequest? filtros, Guid codigoCliente, IEnumerable<int> statusDosPedidos, bool apenasPedidosRetirados = false);
    Task<ProjecaoDoPedidoDoCliente> ObterPedidoDoCliente(UsuarioAutenticado usuarioAutenticado, Guid idPedido);
    Task<IEnumerable<ProjecaoDeSituacaoDoPedido>> ObterSituacoesListadasParaCliente();
    Task<IEnumerable<ProjecaoDeFormaDePagamentoDoPedido>> ObterFormasDePagamentoDoPedido(Guid idPedido);
    Task<Guid> AdicionarAoPedidoEmAndamento(UsuarioAutenticado usuario, DtoAdicionarAoPedidoDoCliente pedido);
    Task AtualizarPedido(UsuarioAutenticado usuarioAutenticado, Guid idPedido, DtoDoPedidoDoCliente pedido);
    Task SolicitarAjudaParaPedido(UsuarioAutenticado clienteAutenticado, Guid idPedido, DtoSolicitacaoDeAjuda ajuda);
    Task CancelarPedido(UsuarioAutenticado cliente, Guid idPedido);
}
