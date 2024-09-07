namespace Dashdine.Service.Models.Cliente.Pedido;

public sealed record DtoDeProdutoDoPedidoDoCliente(Guid IdProduto, string Nome, int Quantidade, IEnumerable<DtoDeAdicionalDeProdutoDoPedidoDoCliente>? Adicionais);