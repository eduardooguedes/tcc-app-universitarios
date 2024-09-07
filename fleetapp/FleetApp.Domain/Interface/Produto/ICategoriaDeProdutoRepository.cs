using Dashdine.Domain.Domain.Produto;
using Dashdine.Domain.Interfafe;

namespace Dashdine.Domain.Interface.Produto;

public interface ICategoriaDeProdutoRepository : IBaseRepository<Entitys.Categorium>
{
    Task<CategoriaDeProduto?> Obter(int id);
    Task<IEnumerable<CategoriaDeProduto>> ObterTodos();
}
