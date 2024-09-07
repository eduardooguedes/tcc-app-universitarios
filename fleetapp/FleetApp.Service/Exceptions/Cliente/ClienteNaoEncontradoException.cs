namespace Dashdine.Service.Exceptions.Cliente;

public sealed class ClienteNaoEncontradoException : ServiceException
{
    public ClienteNaoEncontradoException() : base("Cliente não encontrado.") { }
}
