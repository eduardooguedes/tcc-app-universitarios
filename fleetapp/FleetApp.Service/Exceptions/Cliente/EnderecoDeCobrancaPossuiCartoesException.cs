namespace Dashdine.Service.Exceptions.Cliente;

public sealed class EnderecoDeCobrancaPossuiCartoesException : ServiceException
{
    public EnderecoDeCobrancaPossuiCartoesException() : base("Esse endereço de cobrança possui um ou mais cartões vinculados.") { }
}