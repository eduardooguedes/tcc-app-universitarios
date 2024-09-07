using Newtonsoft.Json;

namespace Dashdine.Domain.Domain.Integracoes.PagSeguro;

public sealed class PedidoPagSeguroDomain
{
    [JsonProperty("id")]
    public string? IdPedidoPagSeguro { get; set; }

    [JsonProperty("reference_id")]
    public string IdPedido { get; set; }

    [JsonProperty("customer")]
    public ClientePagSeguroDomain Cliente { get; set; }

    [JsonProperty("qr_codes")]
    public IEnumerable<QrCodePagSeguroDomain> QrCodes { get; set; }

    [JsonProperty("charges")]
    public IEnumerable<CobrancaPagSeguroDomain> Cobrancas { get; set; }

    [JsonProperty("notification_urls")]
    public IEnumerable<string> UrlsParaNotificacao { get; set; }
}