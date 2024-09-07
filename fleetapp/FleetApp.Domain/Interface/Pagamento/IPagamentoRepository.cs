using Dashdine.Domain.Domain.Pagamento;
using Dashdine.Domain.Domain.Pedido;

namespace Dashdine.Domain.Interface.Pagamento;

public interface IPagamentoRepository
{
    Task<PagamentoPedidoDomain?> SalvarPagamentoPix(PedidoDomain pedido, DateTime dataHora, int minutosParaPagar);
    Task<PagamentoPedidoDomain?> ObterUltimoPagamento(PedidoDomain pedido, DateTime dataHoraAtual);
    Task<bool> Cancelar(PagamentoPedidoDomain pedido, DateTime dataHoraAtual);
}