namespace Dashdine.Domain.Domain.Integracoes.TomTom;

public sealed class TomTomGeocodingApiResponse
{
    public TomTomSummary Summary { get; set; }
    public List<TomTomResult> Results { get; set; }
}