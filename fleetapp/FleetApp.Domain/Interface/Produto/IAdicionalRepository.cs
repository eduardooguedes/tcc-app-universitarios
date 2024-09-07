using Dashdine.Domain.Domain.Produto;
using Dashdine.Domain.Interfafe;

namespace Dashdine.Domain.Interface.Produto;

public interface IAdicionalRepository : IBaseRepository<Dashdine.Domain.Entitys.AdicionalProduto>
{
    public Task<Adicional?> ObterPorId(Guid id);
    public Task<List<Domain.Produto.ProdutoDomain>> ObterProdutosVinculadosOuException(Guid id);
    public Task<List<Adicional>> ObterTodos(Guid idEstabelecimento);
    /// <summary>
    /// Retorna a quantidade solicitada de adicionais, ordenados por nome.
    /// </summary>
    /// <param name="filtroDeNome"></param>
    /// <param name="quantidadeDeAdicionaisRetornados"></param>
    /// <returns></returns>
    Task<List<Adicional>> ObterPorFiltro(string filtroDeNome, int quantidadeDeAdicionaisRetornados);
    public Task<Guid> Cadastrar(Adicional adicional);
    public Task Atualizar(Adicional adicional);
    public Task RemoverVinculo(Guid idAdicional, Guid idProduto);
}
