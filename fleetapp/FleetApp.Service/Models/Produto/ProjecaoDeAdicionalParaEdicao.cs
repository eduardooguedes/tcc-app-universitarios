using Dashdine.Domain.Domain.Produto;

namespace Dashdine.Service.Models.Produto;

public class ProjecaoDeAdicionalParaEdicao
{
    public string Id { get; set; }
    public SituacaoDeProdutoDomain Situacao { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public IEnumerable<ProjecaoParaListagemDeProduto>? ProdutosQuePossuemAdicional { get; set; }
}
