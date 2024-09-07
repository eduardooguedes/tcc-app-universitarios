namespace Dashdine.Service.Exceptions.Cliente;

internal class EnderecoDoClienteNaoEncontradoException : ServiceException
{
    public EnderecoDoClienteNaoEncontradoException() : base("Endereço não encontrado.") { }
}
