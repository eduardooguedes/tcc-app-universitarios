using Dashdine.Domain.Interface.Produto;
using Dashdine.Service.Interface.Produto;
using Dashdine.Service.Models.Produto;

namespace Dashdine.Service.Services.Produto;

public class ConfiguracoesProdutoService : IConfiguracoesProdutoService
{
    private readonly ICategoriaDeProdutoRepository categoriaRepository;
    private readonly ITipoDeProdutoRepository tipoRepository;

    public ConfiguracoesProdutoService(ICategoriaDeProdutoRepository categoriaRepository, ITipoDeProdutoRepository tipoRepository)
    {
        this.categoriaRepository = categoriaRepository;
        this.tipoRepository = tipoRepository;
    }

    public async Task<DtoConfiguracoesDeProduto> ObterTodas()
    {
        return new DtoConfiguracoesDeProduto()
        {
            CategoriasDeProduto = await categoriaRepository.ObterTodos(),
            TiposDeProduto = await tipoRepository.ObterTodos(),
        };
    }
}