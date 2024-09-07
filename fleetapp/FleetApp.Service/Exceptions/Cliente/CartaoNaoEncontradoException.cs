namespace Dashdine.Service.Exceptions.Cliente;

public sealed class CartaoNaoEncontradoException : ServiceException
{
    public CartaoNaoEncontradoException() : base("Cartão não encontrado."){}
}
