namespace Dashdine.Service.Exceptions.Cliente;

public sealed class EnderecoDeRetiradaPossuiPedidosException : ServiceException
{
    public EnderecoDeRetiradaPossuiPedidosException() : base("Essa localização possui um ou mais pedidos vinculados.") { }
}