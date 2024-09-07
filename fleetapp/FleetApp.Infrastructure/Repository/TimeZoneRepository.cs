using Dashdine.Domain.Entitys.Views;
using Dashdine.Domain.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dashdine.Infrastructure.Repository;

public sealed class TimeZoneRepository(IConfiguration configuration) : BaseRepository<Timezone>(configuration), ITimeZoneRepository
{
    public async Task<Timezone> Obter(string timeZone) => await UnitOfWork.Timezone.FromSqlRaw($"SELECT TIMEZONE('{timeZone}', NOW())::timestamptz AS DATA_HORA_ATUAL").FirstAsync();
}
