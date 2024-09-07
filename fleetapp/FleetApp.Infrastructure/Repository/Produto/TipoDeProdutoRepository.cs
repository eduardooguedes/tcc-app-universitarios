using Dashdine.Domain.Domain.Produto;
using Dashdine.Domain.Interface.Produto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dashdine.Infrastructure.Repository.Produto;

public sealed class TipoDeProdutoRepository : BaseRepository<Domain.Entitys.TipoProduto>, ITipoDeProdutoRepository
{
    public TipoDeProdutoRepository(IConfiguration configuration) : base(configuration) { }

    public async Task<TipoDoProdutoDomain?> Obter(int id) => await UnitOfWork.TipoProdutos.Where(i => i.Id == id).Select(t => new TipoDoProdutoDomain(t.Id, t.Descricao)).FirstOrDefaultAsync();
    public async Task<IEnumerable<TipoDoProdutoDomain>> ObterTodos() => await UnitOfWork.TipoProdutos.Select(t => new TipoDoProdutoDomain(t.Id, t.Descricao)).ToListAsync();

}
