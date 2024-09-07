namespace Dashdine.Service.Exceptions.Estabelecimento;

public sealed class ProdutoNaoEncontradoException : ServiceException
{
    public ProdutoNaoEncontradoException(string? nome = null) : base($"Produto{(string.IsNullOrEmpty(nome) ? string.Empty : $" {nome}")} não encontrado.") { }
}
