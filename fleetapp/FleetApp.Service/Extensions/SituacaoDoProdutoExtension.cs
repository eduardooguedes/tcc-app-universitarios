using Dashdine.Domain.Domain.Produto;
using Dashdine.Service.Models.Produto;

namespace Dashdine.Service.Extensions;

public static class SituacaoDoProdutoExtension
{
    public static ProjecaoDeSituacaoDoProduto AsProjecaoDeSituacaoDoProduto(this SituacaoDeProdutoDomain domain) => new(domain.Id, domain.Descricao);
}
