namespace Dashdine.Service.Exceptions.Estabelecimento;

public sealed class NaoEhPossivelDesfazerEntregaDoPedidoException : ServiceException
{
    public NaoEhPossivelDesfazerEntregaDoPedidoException() : base("Não é mais possível desfazer a entrega desse pedido.") { }
}