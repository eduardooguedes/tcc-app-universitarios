namespace Dashdine.Service.Exceptions.Cliente.Pedido;

public sealed class NenhumPagamentoEncontradoParaPedidoException : ServiceException
{
    public NenhumPagamentoEncontradoParaPedidoException() : base("Nenhum pagamento encontrado para o pedido.") { }
}
