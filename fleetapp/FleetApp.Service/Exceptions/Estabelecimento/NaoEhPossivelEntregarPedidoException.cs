namespace Dashdine.Service.Exceptions.Estabelecimento;

public sealed class NaoEhPossivelEntregarPedidoException : ServiceException
{
    public NaoEhPossivelEntregarPedidoException() : base("Não é mais possível entregar esse pedido.") { }
}
