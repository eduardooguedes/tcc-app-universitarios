namespace Dashdine.Domain.Domain.Produto;

public class AdicionalDoProdutoDomain : Adicional
{
    public int? QuantidadeMaxima { get; set; }
    public AdicionalDoProdutoDomain(Guid id, Guid idEstabelecimento, int? quantidadeMaxima) : base(id, idEstabelecimento)
    {
        QuantidadeMaxima = quantidadeMaxima;
    }

    public AdicionalDoProdutoDomain(Guid id, Guid idEstabelecimento, SituacaoDeProdutoDomain situacao, string nome, decimal preco, int? quantidadeMaxima)
        : base(id, idEstabelecimento, situacao, nome, preco)
    {
        QuantidadeMaxima = quantidadeMaxima;
    }
}