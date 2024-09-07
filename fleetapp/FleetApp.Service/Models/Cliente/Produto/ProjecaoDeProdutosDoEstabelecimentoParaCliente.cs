using Dashdine.Service.Models.Produto;

namespace Dashdine.Service.Models.Cliente.Produto;

public sealed record ProjecaoDeProdutosDoEstabelecimentoParaCliente(Guid IdProduto, string Nome, string? Descricao, decimal PrecoUnitario, string? Imagem, ProjecaoDeSituacaoDoProduto Situacao, IEnumerable<ProjecaoDeAdicionalDoProdutoDoEstabelecimentoParaCliente>? Adicionais);