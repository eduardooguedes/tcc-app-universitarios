using Newtonsoft.Json;

namespace Dashdine.Domain.Domain.Integracoes.PagSeguro;

public sealed class QuantiaPagSeguroDomain
{
    [JsonProperty("value")]
    private string valor { get; set; }

    [JsonIgnore]
    public decimal ValorDecimal
    {
        get => decimal.Parse(valor) / 100;
        set
        {
            valor = (value * 100).ToString().Replace(".00", "");
        }
    }
}

//"currency": "BRL",
//"summary": {
//    "total": 200,
//    "paid": 200,
//    "refunded": 0