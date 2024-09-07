using Dashdine.Service.Models.Produto;

namespace Dashdine.Service.Models.Cliente.Produto;

public sealed record ProjecaoDeAdicionalDoProdutoDoEstabelecimentoParaCliente(Guid Id, ProjecaoDeSituacaoDoProduto Situacao, string? Nome, decimal? PrecoUnitario, int? QuantidadeMaxima);