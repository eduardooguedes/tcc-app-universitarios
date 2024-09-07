using System.Text.Json.Serialization;

namespace Dashdine.Domain.Domain.Integracoes.TomTom;

public class TomTomPosition
{
    [JsonPropertyName("lat")]
    public decimal Latitude { get; set; }

    [JsonPropertyName("lon")]
    public decimal Longitude { get; set; }
}
