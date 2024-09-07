using Dashdine.Domain.Domain.Pagamento;
using Dashdine.Domain.Interfafe;

namespace Dashdine.Domain.Interface.Pagamento;

public interface ISituacaoPagamentoRepository: IBaseRepository<Entitys.SituacaoPagamento>
{
    Task<SituacaoPagamentoDomain> ObterSituacaoAguardando();
    Task<SituacaoPagamentoDomain> ObterSituacaoCancelado();
    Task<SituacaoPagamentoDomain> ObterSituacaoExpirado();
}
