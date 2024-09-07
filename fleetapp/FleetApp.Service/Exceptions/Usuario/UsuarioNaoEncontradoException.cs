namespace Dashdine.Service.Exceptions.Usuario;

public sealed class UsuarioNaoEncontradoException : ServiceException
{
    public UsuarioNaoEncontradoException() : base("Usuário não encontrado.") { }
}
