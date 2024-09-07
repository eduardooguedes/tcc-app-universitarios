namespace Dashdine.Service.Exceptions.Cliente.Pedido;

public sealed class ProdutoSemAdicionaisException : ServiceException
{
    public ProdutoSemAdicionaisException(string nome) : base($"Produto {nome} não possui adicionais.") { }
}
