namespace Dashdine.Service.Models.Cliente.Produto;

public sealed record ProjecaoDeCategoriaDeProdutosDoEstabelecimentoParaCliente(int Id, string Descricao, IEnumerable<ProjecaoDeProdutosDoEstabelecimentoParaCliente> Produtos);
