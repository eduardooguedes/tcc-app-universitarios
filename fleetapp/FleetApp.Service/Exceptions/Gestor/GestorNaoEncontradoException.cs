namespace Dashdine.Service.Exceptions.Gestor;

public sealed class GestorNaoEncontradoException : ServiceException
{
    public GestorNaoEncontradoException() : base("Gestor não encontrado.")
    {
    }
}
