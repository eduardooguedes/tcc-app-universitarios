using Dashdine.Domain.Domain.Produto;
using Dashdine.Domain.Interfafe;

namespace Dashdine.Domain.Interface.Produto;

public interface ITipoDeProdutoRepository : IBaseRepository<Entitys.TipoProduto>
{
    Task<TipoDoProdutoDomain?> Obter(int id);
    Task<IEnumerable<TipoDoProdutoDomain>> ObterTodos();
}
