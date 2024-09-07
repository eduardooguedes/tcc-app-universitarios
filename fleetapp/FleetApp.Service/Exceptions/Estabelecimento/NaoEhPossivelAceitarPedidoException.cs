namespace Dashdine.Service.Exceptions.Estabelecimento;

public sealed class NaoEhPossivelAceitarPedidoException : ServiceException
{
    public NaoEhPossivelAceitarPedidoException() : base("Não é mais possível aceitar esse pedido.") { }
}
