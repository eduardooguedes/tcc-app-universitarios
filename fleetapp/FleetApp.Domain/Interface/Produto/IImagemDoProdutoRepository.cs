using Microsoft.AspNetCore.Http;

namespace Dashdine.Domain.Interface.Produto;

public interface IImagemDoProdutoRepository
{
    /// <summary>
    /// Insere ou atualiza imagem. Retorna url da imagem.
    /// </summary>
    Task<string> AtualizarImagem(Guid idEstabelecimento, Guid idProduto, IFormFile imagem);
    Task RemoverImagem(Guid idEstabelecimento, Guid idProduto);
}
