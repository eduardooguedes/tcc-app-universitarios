namespace Dashdine.Service.Exceptions.Usuario;

public sealed class UsuarioNaoPermitidoARecuperarSenhaException : ServiceException
{
    public UsuarioNaoPermitidoARecuperarSenhaException() : base("Usuário não está autorizado a recuperar senha.") { }
}
