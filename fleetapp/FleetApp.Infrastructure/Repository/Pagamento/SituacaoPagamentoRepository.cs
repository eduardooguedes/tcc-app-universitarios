using Dashdine.Domain.Domain.Pagamento;
using Dashdine.Domain.Entitys;
using Dashdine.Domain.Interface.Pagamento;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dashdine.Infrastructure.Repository.Pagamento;

public sealed class SituacaoPagamentoRepository(IConfiguration configuration) : BaseRepository<SituacaoPagamento>(configuration), ISituacaoPagamentoRepository
{
    public async Task<SituacaoPagamentoDomain> ObterSituacaoAguardando() => await UnitOfWork.SituacaoPagamentos.AsQueryable().Where(s => s.Aguardando).Select(s => ToDomain(s)).FirstAsync();
    public async Task<SituacaoPagamentoDomain> ObterSituacaoCancelado() => await UnitOfWork.SituacaoPagamentos.AsQueryable().Where(s => s.Cancelado).Select(s => ToDomain(s)).FirstAsync();
    public async Task<SituacaoPagamentoDomain> ObterSituacaoExpirado() => await UnitOfWork.SituacaoPagamentos.AsQueryable().Where(s => s.Expirado).Select(s => ToDomain(s)).FirstAsync();

    private static SituacaoPagamentoDomain ToDomain(SituacaoPagamento s) => new SituacaoPagamentoDomain(s.Id, s.Descricao, s.Aguardando, s.EmAnalise, s.Pago, s.Cancelado, s.Rejeitado, s.Expirado, s.EstornarAoCancelarPedido);
}
