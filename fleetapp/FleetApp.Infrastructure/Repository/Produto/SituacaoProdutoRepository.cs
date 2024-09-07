using Dashdine.CrossCutting.Enums;
using Dashdine.Domain.Domain.Produto;
using Dashdine.Domain.Interface.Produto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dashdine.Infrastructure.Repository.Produto;

public sealed class SituacaoProdutoRepository(IConfiguration configuration) : BaseRepository<Domain.Entitys.SituacaoProduto>(configuration), ISituacaoProdutoRepository
{
    public async Task<SituacaoDeProdutoDomain?> Obter(int id) =>
        await UnitOfWork.SituacaoProdutos
        .Select(s => new SituacaoDeProdutoDomain(s.Id, s.Descricao))
        .FirstOrDefaultAsync(s => s.Id.Equals(id));

    public async Task<SituacaoDeProdutoDomain> ObterAtivo() =>
        await UnitOfWork.SituacaoProdutos
        .Where(s => s.Ativo)
        .Select(s => new SituacaoDeProdutoDomain(s.Id, s.Descricao))
        .FirstAsync();
}
