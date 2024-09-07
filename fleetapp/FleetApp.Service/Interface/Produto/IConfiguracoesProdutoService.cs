using Dashdine.Service.Models.Produto;

namespace Dashdine.Service.Interface.Produto;

public interface IConfiguracoesProdutoService
{
    Task<DtoConfiguracoesDeProduto> ObterTodas();
}
