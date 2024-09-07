namespace Dashdine.Domain.Domain.Pedido;

public sealed record ClienteDoPedidoDomain(Guid Id, string Nome, string Cpf, string Email, string Celular);