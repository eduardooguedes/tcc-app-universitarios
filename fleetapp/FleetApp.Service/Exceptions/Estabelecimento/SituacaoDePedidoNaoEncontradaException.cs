namespace Dashdine.Service.Exceptions.Estabelecimento;

public sealed class SituacaoDePedidoNaoEncontradaException : ServiceException
{
    public SituacaoDePedidoNaoEncontradaException(int? situacao) : base($"Situação{(situacao is not null ? $" {situacao}" : "")} de pedido informada não existe.") { }
}
