using Dashdine.Domain.Domain;
using Dashdine.Domain.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dashdine.Infrastructure.Repository;

public sealed class ParametroRepository(IConfiguration configuration) : BaseRepository<Domain.Entitys.Parametro>(configuration), IParametroRepository
{
    private async Task<ParametroDomain> Obter(int id) =>
        await UnitOfWork.Parametros
        .AsQueryable()
        .Where(p => p.Id.Equals(id))
        .Select(p => new ParametroDomain(p.Id, p.Descricao, p.Valor))
        .FirstAsync();

    public async Task<string> ObterLogoPadraoEstabelecimento() => (await Obter(1)).Valor;
    public async Task<string> ObterImagemPadraoProduto() => (await Obter(2)).Valor;
    public async Task<int> ObterSegundosPadraoExpiracaoPix() => int.Parse((await Obter(3)).Valor);
}
