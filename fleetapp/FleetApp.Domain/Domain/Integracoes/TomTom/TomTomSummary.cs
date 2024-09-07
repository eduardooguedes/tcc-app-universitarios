using System.Text.Json.Serialization;

namespace Dashdine.Domain.Domain.Integracoes.TomTom;

public class TomTomSummary
{
    [JsonPropertyName("numResults")]
    public int NumeroDeResultados { get; set; }

    [JsonPropertyName("totalResults")]
    public int TotalDeResultados { get; set; }

    [JsonPropertyName("offset")]
    public int OffSet { get; set; }

    [JsonPropertyName("fuzzyLevel")]
    public int FuzzyLevel { get; set; }
}
