namespace Dashdine.Service.Exceptions.Cliente.Pedido;

public sealed class PedidoNaoEncontradoException : ServiceException
{
    public PedidoNaoEncontradoException() : base("Pedido não encontrado.") { }
}
