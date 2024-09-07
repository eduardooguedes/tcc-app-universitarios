using Dashdine.Domain.Domain.Produto;
using Dashdine.Service.Models.Cliente.Produto;

namespace Dashdine.Service.Extensions;

public static class AdicionalDoProdutoDoEstabelecimentoParaClienteExtension
{
    public static ProjecaoDeAdicionalDoProdutoDoEstabelecimentoParaCliente AsProjecaoDeAdicionalDoProdutoDoEstabelecimentoParaCliente(this AdicionalDoProdutoDomain domain) => new(domain.Id, domain.Situacao.AsProjecaoDeSituacaoDoProduto(), domain.Nome, domain.Preco, domain.QuantidadeMaxima);
}
