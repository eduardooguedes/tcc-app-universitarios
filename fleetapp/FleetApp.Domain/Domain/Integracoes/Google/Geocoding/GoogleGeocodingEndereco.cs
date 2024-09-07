using Newtonsoft.Json;

namespace Dashdine.Domain.Domain.Integracoes.Google.Geocoding;

public class GoogleGeocodingEndereco
{
    [JsonProperty("place_id")]
    public string IdDoEndereco { get; set; }

    [JsonProperty("address_components")]
    public List<GoogleGeocodingComponenteDoEndereco> ComponentesDoEndereco { get; set; }

    [JsonProperty("formatted_address")]
    public string EnderecoCompleto { get; set; }

    [JsonProperty("geometry")]
    public GoogleGeocodingGeometriaDoEndereco Geometrias { get; set; }

    [JsonProperty("types")]
    public List<string> Tipos { get; set; }

}
