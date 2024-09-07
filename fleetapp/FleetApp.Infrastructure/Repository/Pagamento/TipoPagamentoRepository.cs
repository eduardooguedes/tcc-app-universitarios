using Dashdine.Domain.Domain.Pagamento;
using Dashdine.Domain.Entitys;
using Dashdine.Domain.Interface.Pagamento;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dashdine.Infrastructure.Repository.Pagamento;

public sealed class TipoPagamentoRepository(IConfiguration configuration) : BaseRepository<Domain.Entitys.TipoPagamento>(configuration), ITipoPagamentoRepository
{
    public async Task<TipoPagamentoDomain?> ObterTipoPagamentoPix() => await UnitOfWork.TipoPagamentos.AsQueryable().Where(t => t.Pix).Select(t => ToDomain(t)).FirstOrDefaultAsync();

    private static TipoPagamentoDomain ToDomain(TipoPagamento tipo) => new TipoPagamentoDomain(tipo.Id, tipo.Descricao, tipo.CartaoCredito, tipo.Pix);
}
