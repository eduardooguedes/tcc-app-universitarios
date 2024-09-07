namespace Dashdine.Service.Models.Cliente.Pedido;

public sealed record ProjecaoDeEstabelecimentoDoPedidoDoCliente(Guid Id, string? Logo, string NomeFantasia, string EnderecoCompleto, decimal? Latitude, decimal? Longitude);
