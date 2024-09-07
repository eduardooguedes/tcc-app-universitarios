namespace Dashdine.Domain.Domain.Integracoes.Google.Timezone;

public sealed class GoogleTimezoneResult
{
    public int DstOffset { get; set; }
    public decimal RawOffset { get; set; }
    public string Status { get; set; }
    public string TimeZoneId { get; set; }
    public string TimeZoneName { get; set; }
}