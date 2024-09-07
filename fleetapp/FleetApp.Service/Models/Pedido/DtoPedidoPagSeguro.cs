using Dashdine.Domain.Domain.Integracoes.PagSeguro;
using Newtonsoft.Json;

namespace Dashdine.Service.Models.Pedido;

public class DtoPedidoPagSeguro
{
    [JsonProperty("id")]
    public string? IdPedidoPagSeguro { get; set; }

    [JsonProperty("reference_id")]
    public string IdPedido { get; set; }

    [JsonProperty("customer")]
    public ClientePagSeguroDomain Cliente { get; set; }

    [JsonProperty("charges")]
    public IEnumerable<CobrancaPagSeguroDomain> Cobrancas { get; set; }
}
