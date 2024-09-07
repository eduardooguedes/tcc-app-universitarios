using Dashdine.Domain.Domain.Pagamento;
using Dashdine.Service.Models.Cliente.Pedido;

namespace Dashdine.Service.Extensions;

public static class PagamentoDoPedidoExtension
{
    public static ProjecaoDeSituacaoDePagamento AsProjecao(this SituacaoPagamentoDomain domain) => new(domain.Id, domain.Descricao, domain.Aguardando, domain.EmAnalise, domain.Pago, domain.Cancelado, domain.Rejeitado, domain.Expirado);
}
