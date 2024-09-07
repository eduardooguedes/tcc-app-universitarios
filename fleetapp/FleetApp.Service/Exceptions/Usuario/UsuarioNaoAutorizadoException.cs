namespace Dashdine.Service.Exceptions.Usuario;

public sealed class UsuarioNaoAutorizadoException : ServiceException
{
    public UsuarioNaoAutorizadoException() : base("Usuário não autorizado.") { }
}
