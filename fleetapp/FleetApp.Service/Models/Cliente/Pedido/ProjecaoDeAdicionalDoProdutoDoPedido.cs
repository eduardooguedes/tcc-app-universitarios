using Dashdine.Service.Models.Produto;

namespace Dashdine.Service.Models.Cliente.Pedido;

public sealed record ProjecaoDeAdicionalDoProdutoDoPedido(Guid Id, ProjecaoDeSituacaoDoProduto Situacao, string Nome, decimal PrecoUnitario, decimal ValorTotal, int Quantidade, int? QuantidadeMaxima);
