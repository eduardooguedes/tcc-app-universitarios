using Dashdine.Domain.Interfafe;

namespace Dashdine.Domain.Interface;

public interface IParametroRepository : IBaseRepository<Entitys.Parametro>
{
    Task<string> ObterLogoPadraoEstabelecimento();
    Task<string> ObterImagemPadraoProduto();
    Task<int> ObterSegundosPadraoExpiracaoPix();
}
