namespace Dashdine.Service.Exceptions.Estabelecimento;

public sealed class AdicionalNaoEncontradoException : ServiceException
{
    public AdicionalNaoEncontradoException(string? nome = null, string? nomeProduto = null) : base($"Adicional{(string.IsNullOrEmpty(nome) ? "" : $" {nome}")} não encontrado{(string.IsNullOrEmpty(nomeProduto) ? "" : $" no produto {nomeProduto}")}.")
    {
    }
}
