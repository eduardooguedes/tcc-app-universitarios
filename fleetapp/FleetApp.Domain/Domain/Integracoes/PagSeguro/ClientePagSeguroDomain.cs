using Newtonsoft.Json;

namespace Dashdine.Domain.Domain.Integracoes.PagSeguro;

public sealed class ClientePagSeguroDomain
{
    [JsonProperty("name")]
    public string Nome { get; set; }
    
    [JsonProperty("email")]
    public string Email { get; set; }
    
    [JsonProperty("tax_id")]
    public string Cpf { get; set; }
    
    [JsonProperty("phones")]
    public IEnumerable<TelefonePagSeguroDomain> Telefones { get; set; }
}