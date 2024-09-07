namespace Dashdine.Service.Exceptions.Estabelecimento;

public sealed class SituacaoDeProdutoNaoEncontradaException : ServiceException
{
    public SituacaoDeProdutoNaoEncontradaException() : base("Situação de produto não encontrado.")
    {
    }
}
