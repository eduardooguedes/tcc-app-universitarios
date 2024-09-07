using Dashdine.Domain.Domain.Produto;
using Dashdine.Domain.Interface.Produto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dashdine.Infrastructure.Repository.Produto;

public sealed class CategoriaDeProdutoRepository(IConfiguration configuration) : BaseRepository<Domain.Entitys.Categorium>(configuration), ICategoriaDeProdutoRepository
{
    public async Task<CategoriaDeProduto?> Obter(int id) => await UnitOfWork.Categoria.Where(i => i.Id == id).Select(t => new CategoriaDeProduto(t.Id, t.Descricao)).FirstOrDefaultAsync();
    public async Task<IEnumerable<CategoriaDeProduto>> ObterTodos() => await UnitOfWork.Categoria.Select(t => new CategoriaDeProduto(t.Id, t.Descricao)).ToListAsync();
}
