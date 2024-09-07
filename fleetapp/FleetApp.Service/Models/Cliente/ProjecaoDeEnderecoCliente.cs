namespace Dashdine.Service.Models.Cliente;

public class ProjecaoDeEnderecoCliente
{
    public string Id { get; set; }

    /// <summary>
    /// 1 = Retirada | 2 = Pagamento
    /// </summary>
    public int Tipo { get; set; }
    public bool Principal { get; set; }
    public string Apelido { get; set; }
    public string? CEP { get; set; }
    public string? Logradouro { get; set; }
    public int? Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}
