using Dashdine.Domain.Domain.Estabelecimento.HorarioDeFuncionamento;

namespace Dashdine.Service.Models.Cliente.Pedido;

public sealed record DtoDoPedidoDoCliente(
    IEnumerable<DtoDeProdutoDoPedidoDoCliente> Produtos,
    string? Observacao,
    DateTime? DataHoraARetirar,
    DtoDestinoDaRetirada? DestinoPedido);