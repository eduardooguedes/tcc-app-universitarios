namespace Dashdine.Service.Exceptions.Cliente.Pedido;

public sealed class NaoEhPossivelCancelarPedidoException : ServiceException
{
    public NaoEhPossivelCancelarPedidoException() : base("Não é mais possível cancelar esse pedido.") { }
}
