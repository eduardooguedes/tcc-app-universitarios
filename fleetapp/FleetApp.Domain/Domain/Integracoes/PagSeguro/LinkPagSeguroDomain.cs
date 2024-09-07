using Newtonsoft.Json;

namespace Dashdine.Domain.Domain.Integracoes.PagSeguro;

public sealed class LinkPagSeguroDomain
{
    [JsonProperty("rel")]
    public string Relacao { get; set; }

    [JsonProperty("href")]
    public string Href { get; set; }

    [JsonProperty("media")]
    public string Media { get; set; }

    [JsonProperty("type")]
    public string Tipo { get; set; }
}