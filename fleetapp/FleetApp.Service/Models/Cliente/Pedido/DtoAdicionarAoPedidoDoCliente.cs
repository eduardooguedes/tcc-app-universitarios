namespace Dashdine.Service.Models.Cliente.Pedido;

public sealed record DtoAdicionarAoPedidoDoCliente(Guid IdLocalizacaoCliente, Guid IdEstabelecimento, IEnumerable<DtoDeProdutoDoPedidoDoCliente> Produtos);
