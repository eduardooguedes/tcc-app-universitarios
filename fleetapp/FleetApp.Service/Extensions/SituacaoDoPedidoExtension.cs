using Dashdine.Domain.Domain.Pedido;
using Dashdine.Service.Models.Cliente.Pedido;

namespace Dashdine.Service.Extensions;

public static class SituacaoDoPedidoExtension
{
    public static ProjecaoDeSituacaoDoPedido AsProjecao(this SituacaoDoPedidoDomain domain) => new(domain.Id, domain.Descricao, domain.CorHexadecimal);
}
