using Microsoft.AspNetCore.Http;

namespace Dashdine.Domain.Interface.Estabelecimento;

public interface ILogoDoEstabelecimentoRepository
{
    /// <summary>
    /// Insere ou atualiza logo. Retorna url do objeto.
    /// </summary>
    /// <param name="idEstabelecimento"></param>
    /// <param name="imagem"></param>
    /// <returns></returns>
    Task<string> AtualizarLogo(Guid idEstabelecimento, IFormFile imagem);
    Task RemoverLogo(Guid idEstabelecimento);
}
