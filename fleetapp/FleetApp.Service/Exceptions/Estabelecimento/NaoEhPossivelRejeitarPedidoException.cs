namespace Dashdine.Service.Exceptions.Estabelecimento;

public sealed class NaoEhPossivelRejeitarPedidoException : ServiceException
{
    public NaoEhPossivelRejeitarPedidoException() : base("Não é mais possível rejeitar esse pedido.")
    {
    }
}