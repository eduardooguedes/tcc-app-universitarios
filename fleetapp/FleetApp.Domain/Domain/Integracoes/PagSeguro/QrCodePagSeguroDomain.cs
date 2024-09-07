using Newtonsoft.Json;

namespace Dashdine.Domain.Domain.Integracoes.PagSeguro;

public sealed class QrCodePagSeguroDomain
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("amount")]
    public QuantiaPagSeguroDomain Quantia { get; set; }

    [JsonProperty("expiration_date")]
    private string dataExpiracao;

    [JsonIgnore]
    public DateTime DataExpiracao
    {
        get { return DateTime.Parse(dataExpiracao); }
        set { dataExpiracao = value.ToString("yyyy-MM-ddTHH:mm:ss-03:00"); }
    }

    [JsonProperty("text")]
    public string? Texto { get; set; }

    [JsonProperty("links")]
    public IEnumerable<LinkPagSeguroDomain> Links { get; set; }
}