using Dashdine.Domain.Domain.Produto;

namespace Dashdine.Domain.Domain.Pedido;

public sealed class ProdutoDoPedidoDomain
{
    public Guid Id { get; }
    public Guid IdProduto { get; }
    public SituacaoDeProdutoDomain Situacao { get; }
    public TipoDoProdutoDomain Tipo { get; }
    public string Nome { get; }
    public string? Descricao { get; }
    public string? Imagem { get; }
    public int Quantidade { get; }
    public decimal PrecoProduto { get; }
    public decimal PrecoUnitario { get; }
    /// <summary>
    /// Quantidade vezes preço do produto.
    /// </summary>
    public decimal PrecoTotal { get; }
    public IEnumerable<AdicionalDoProdutoDoPedidoDomain>? Adicionais { get; }

    public ProdutoDoPedidoDomain(Guid id, Guid idProduto, SituacaoDeProdutoDomain situacao, TipoDoProdutoDomain tipo, string nome, string? descricao, string? imagem, int quantidade, decimal precoProduto, decimal precoUnitario, decimal precoTotal, IEnumerable<AdicionalDoProdutoDoPedidoDomain>? adicionais)
    {
        Id = id;
        IdProduto = idProduto;
        Situacao = situacao;
        Tipo = tipo;
        Nome = nome;
        Descricao = descricao;
        Imagem = imagem;
        Quantidade = quantidade;
        PrecoProduto = precoProduto;
        PrecoUnitario = precoUnitario;
        PrecoTotal = precoTotal;
        Adicionais = adicionais;
    }
}
