using Newtonsoft.Json;

namespace Dashdine.Domain.Domain.Integracoes.Google.Geocoding;

public class GoogleGeocodingGeometriaDoEndereco
{
    [JsonProperty("location_type")]
    public string TipoLocalizacao { get; set; }

    [JsonProperty("location")]
    public GoogleGeocodingGeometriaDoEnderecoLocalizacao Localizacao { get; set; }
}
