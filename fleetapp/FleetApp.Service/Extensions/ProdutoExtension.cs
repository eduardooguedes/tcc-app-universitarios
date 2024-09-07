using Dashdine.Domain.Domain.Pedido;
using Dashdine.Domain.Domain.Produto;
using Dashdine.Service.Models.Cliente.Pedido;
using Dashdine.Service.Models.Cliente.Produto;
using Dashdine.Service.Models.Pedido;

namespace Dashdine.Service.Extensions;

public static class ProdutoExtension
{
    public static ProjecaoDeProdutosDoEstabelecimentoParaCliente AsProdutoDoEstabelecimentoParaCliente(this ProdutoDomain domain) => new(domain.Id, domain.Nome, domain.Descricao, domain.Preco, domain.Imagem, domain.Situacao.AsProjecaoDeSituacaoDoProduto(), domain.Adicionais?.Select(a => a.AsProjecaoDeAdicionalDoProdutoDoEstabelecimentoParaCliente()));

    public static ProjecaoDeProdutoDoPedidoParaListagem AsProjecaoParaListagem(this ProdutoDoPedidoDomain domain) => new(domain.Id, domain.Nome, domain.Quantidade, domain.PrecoTotal, new ProjecaoDeTipoDoProdutoDoPedidoParaListagem(domain.Tipo.Id, domain.Tipo.Descricao), domain.Adicionais?.Select(a => a.AsProjecaoParaListagem()));

    public static ProjecaoDeAdicionalDoProdutoDoPedidoParaListagem AsProjecaoParaListagem(this AdicionalDoProdutoDoPedidoDomain domain) => new(domain.IdAdicional, domain.Nome, domain.PrecoTotal, domain.Quantidade);
}
