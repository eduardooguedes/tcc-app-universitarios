using Newtonsoft.Json;

namespace Dashdine.Domain.Domain.Integracoes.Google.Geocoding;

public class GoogleGeocodingGeometriaDoEnderecoLocalizacao
{
    [JsonProperty("lat")]
    public decimal Latitude { get; set; }
    
    [JsonProperty("lng")]
    public decimal Longitude { get; set; }
}
