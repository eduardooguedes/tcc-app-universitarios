namespace Dashdine.Service.Exceptions.Cliente;

public sealed class LocalizacaoDoClienteNaoEncontradaException : ServiceException
{
    public LocalizacaoDoClienteNaoEncontradaException() : base("Localização do cliente não encontrada."){}
}
