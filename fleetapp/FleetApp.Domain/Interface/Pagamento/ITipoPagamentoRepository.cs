using Dashdine.Domain.Domain.Pagamento;
using Dashdine.Domain.Interfafe;

namespace Dashdine.Domain.Interface.Pagamento;

public interface ITipoPagamentoRepository : IBaseRepository<Entitys.TipoPagamento>
{
    Task<TipoPagamentoDomain?> ObterTipoPagamentoPix();
}
