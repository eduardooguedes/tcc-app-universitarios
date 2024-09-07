using Dashdine.Domain.Entitys.Views;

namespace Dashdine.Domain.Interface;

public interface ITimeZoneRepository
{
    Task<Timezone> Obter(string timeZone);
}
