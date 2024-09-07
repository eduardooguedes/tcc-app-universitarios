using Dashdine.Domain.Domain.Produto;

namespace Dashdine.Service.Models.Produto;

public sealed record ProjecaoDeAdicionalParaListagem(string Id, string? Nome, decimal? Preco, SituacaoDeProdutoDomain Situacao, int QuantidadeDeProdutosVinculados);
