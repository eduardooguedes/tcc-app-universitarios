namespace Dashdine.Service.Exceptions.Cliente;

public sealed class TipoDeEnderecoNaoEncontradoException : ServiceException
{
    public TipoDeEnderecoNaoEncontradoException() : base("Tipo de endereço não encontrado.") { }
}
