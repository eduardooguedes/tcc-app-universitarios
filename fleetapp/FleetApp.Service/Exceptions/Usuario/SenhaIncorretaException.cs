namespace Dashdine.Service.Exceptions.Usuario;

public sealed class SenhaIncorretaException : ServiceException
{
    public SenhaIncorretaException() : base("Senha incorreta.") { }
}
