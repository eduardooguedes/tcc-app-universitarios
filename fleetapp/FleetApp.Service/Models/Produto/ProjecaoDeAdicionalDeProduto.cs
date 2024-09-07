using Dashdine.Domain.Domain.Produto;

namespace Dashdine.Service.Models.Produto;

public class ProjecaoDeAdicionalDeProduto
{
    public string Id { get; set; }
    public SituacaoDeProdutoDomain Situacao { get; set; }
    public string Nome { get; set; }
    public decimal PrecoUnitario { get; set; }
    public int? QuantidadeMaxima { get; set; }
}
