namespace Dashdine.Service.Exceptions.Cliente;

public sealed class TipoDoCartaoNaoAceitoException : ServiceException
{
    public TipoDoCartaoNaoAceitoException() : base("Tipo do cartão não aceito."){}
}
