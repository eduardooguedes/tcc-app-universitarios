using Newtonsoft.Json;

namespace Dashdine.Domain.Domain.Integracoes.PagSeguro;

public sealed class CobrancaPagSeguroDomain
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }
    
    [JsonProperty("created_at")]
    public DateTime CriadoEm { get; set; }
    
    [JsonProperty("paid_at")]
    public DateTime PagoEm { get; set; }

    [JsonProperty("amount")]
    public QuantiaPagSeguroDomain Quantia { get; set; }
}