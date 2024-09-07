using Dashdine.Domain.Domain.Produto;

namespace Dashdine.Service.Models.Produto;

public class ProjecaoDeProduto
{
    public string Id { get; set; }
    public CategoriaDeProduto Categoria { get; set; }
    public ProjecaoDeSituacaoDoProduto Situacao { get; set; }
    public TipoDoProdutoDomain Tipo { get; set; }
    public string Nome { get; set; }
    public string? Descricao { get; set; }
    public string? Imagem { get; set; }
    public decimal Preco { get; set; }
    public int TempoEmMinutosParaRetirada { get; set; }
    public int QuantidadeVezesPreparado { get; set; }
    public decimal? NotaMedia { get; set; }
    public List<ProjecaoDeAdicionalDeProduto>? Adicionais { get; set; }
}
