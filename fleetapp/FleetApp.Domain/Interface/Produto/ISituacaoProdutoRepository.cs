using Dashdine.Domain.Domain.Produto;
using Dashdine.Domain.Interfafe;

namespace Dashdine.Domain.Interface.Produto;

public interface ISituacaoProdutoRepository : IBaseRepository<Entitys.SituacaoProduto>
{
    Task<SituacaoDeProdutoDomain?> Obter(int id);
    Task<SituacaoDeProdutoDomain> ObterAtivo();
}
