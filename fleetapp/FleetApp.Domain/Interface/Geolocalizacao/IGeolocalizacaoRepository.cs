using Dashdine.Domain.Domain.Geolocalizacao;

namespace Dashdine.Domain.Interface.Geolocalizacao;

public interface IGeolocalizacaoRepository
{
    Task<GeolocalizacaoDomain?> Obter(string logradouro, int numero, string? complemento, string cidade, string estado);
    Task<GeolocalizacaoDomain?> Obter(decimal latitude, decimal longitude);
}
