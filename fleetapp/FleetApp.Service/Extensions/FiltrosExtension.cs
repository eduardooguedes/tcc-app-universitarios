using Dashdine.Domain.Filtros;
using Dashdine.Service.Models.Filtros;

namespace Dashdine.Service.Extensions;

public static class FiltrosExtension
{
    public static FiltrosDomain ToDomain(this FiltrosRequest request) => new(request.Pagina, request.QuantidadeDeRegistros);
}
