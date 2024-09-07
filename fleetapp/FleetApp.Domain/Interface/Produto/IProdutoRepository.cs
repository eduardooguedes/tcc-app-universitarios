using Dashdine.Domain.Domain.Produto;
using Dashdine.Domain.Interfafe;

namespace Dashdine.Domain.Interface.Produto;

public interface IProdutoRepository : IBaseRepository<Dashdine.Domain.Entitys.Produto>
{
    Task Cadastrar(Domain.Produto.ProdutoDomain produto);
    Task Atualizar(Domain.Produto.ProdutoDomain produto);
    Task<Domain.Produto.ProdutoDomain?> Obter(Guid idProduto);
    Task<IEnumerable<Domain.Produto.ProdutoDomain>> ObterTodos(Guid idEstabelecimento, CategoriaDeProduto? categoria);
}
