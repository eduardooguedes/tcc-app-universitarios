namespace Dashdine.Service.Exceptions.Estabelecimento;

public sealed class CategoriaDoProdutoNaoDisponivelException : ServiceException
{
    public CategoriaDoProdutoNaoDisponivelException() : base("Categoria informada não disponível.")
    {
    }
}
