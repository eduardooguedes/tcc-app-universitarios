namespace Dashdine.Service.Exceptions.Estabelecimento;

public sealed class TipoDoProdutoNaoExisteException : ServiceException
{
    public TipoDoProdutoNaoExisteException() : base("Tipo do produto informado não existe.")
    {
    }
}
