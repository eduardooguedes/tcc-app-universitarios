using Newtonsoft.Json;

namespace Dashdine.Domain.Domain.Integracoes.PagSeguro;

public sealed class TelefonePagSeguroDomain
{
    [JsonProperty("country")]
    public string Pais { get; set; }

    [JsonProperty("area")]
    public string Area { get; set; }

    [JsonProperty("number")]
    public string Numero { get; set; }

    [JsonProperty("type")]
    public readonly string Tipo = "MOBILE";
}