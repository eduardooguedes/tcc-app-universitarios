namespace Dashdine.Service.Models.Estabelecimento;

public class ProjecaoDeEnderecoEstabelecimento
{
    public string Id { get; set; }
    public string CEP { get; set; }
    public string Logradouro { get; set; }
    public int Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}
