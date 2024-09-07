using Dashdine.Service.Models.Cliente.Pedido;

namespace Dashdine.Service.Models.Pedido;

public sealed record ProjecaoDeProdutoDoPedidoParaListagem(Guid Id, string Nome, int Quantidade, decimal PrecoTotal, ProjecaoDeTipoDoProdutoDoPedidoParaListagem Tipo, IEnumerable<ProjecaoDeAdicionalDoProdutoDoPedidoParaListagem>? Adicionais);