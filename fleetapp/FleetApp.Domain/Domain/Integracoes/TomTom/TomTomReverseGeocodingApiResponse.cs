namespace Dashdine.Domain.Domain.Integracoes.TomTom;

public sealed class TomTomReverseGeocodingApiResponse
{
    public TomTomSummary Summary { get; set; }
    public List<TomTomResult> Addresses { get; set; }
}