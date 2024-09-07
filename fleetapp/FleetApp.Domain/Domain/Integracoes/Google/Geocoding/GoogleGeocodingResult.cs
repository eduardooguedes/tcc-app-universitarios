using Newtonsoft.Json;

namespace Dashdine.Domain.Domain.Integracoes.Google.Geocoding;

public class GoogleGeocodingResult
{
    [JsonProperty("Results")]
    public List<GoogleGeocodingEndereco> Enderecos { get; set; }
    
    [JsonProperty("Status")]
    public string Status { get; set; }
}
