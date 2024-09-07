using Dashdine.Service.Models.Produto;

namespace Dashdine.Service.Models.Cliente.Pedido;

public sealed record ProjecaoDeProdutoDoPedido(Guid IdProdutoPedido, Guid IdProduto, string Nome, string? Descricao, int Quantidade, decimal PrecoProduto, decimal PrecoUnitario, decimal PrecoTotal, string? Imagem, ProjecaoDeSituacaoDoProduto Situacao, ProjecaoDeTipoDoProdutoDoPedido Tipo, IEnumerable<ProjecaoDeAdicionalDoProdutoDoPedido>? Adicionais);