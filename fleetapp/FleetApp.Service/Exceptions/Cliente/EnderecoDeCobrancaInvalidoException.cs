namespace Dashdine.Service.Exceptions.Cliente;

public sealed class EnderecoDeCobrancaInvalidoException : ServiceException
{
    public EnderecoDeCobrancaInvalidoException() : base("O endereço de cobrança informado é inválido."){}
}
