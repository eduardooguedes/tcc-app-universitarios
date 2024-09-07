namespace Dashdine.Domain.Domain.Integracoes.TomTom;

public class TomTomResult
{
    public string Id { get; set; }
    public string Type { get; set; }
    public decimal Score { get; set; }
    public TomTomMatchConfidence MatchConfidence { get; set; }
    public TomTomAddress Address { get; set; }
    public TomTomPosition Position { get; set; }
}
