using Newtonsoft.Json;

namespace Dashdine.Domain.Domain.Integracoes.Google.Geocoding;

public class GoogleGeocodingComponenteDoEndereco
{
    [JsonProperty("long_name")]
    public string NomeCompleto { get; set; }

    [JsonProperty("short_name")]
    public string NomeResumido { get; set; }

    [JsonProperty("types")]
    public List<string> Tipos { get; set; }
}
