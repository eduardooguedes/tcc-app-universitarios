namespace Dashdine.Service.Models.Cliente.Pedido;

public sealed record ProjecaoDeFormaDePagamentoDoPedido(Guid? Id, int Tipo, string? Imagem, string? Nome, string? Descricao);