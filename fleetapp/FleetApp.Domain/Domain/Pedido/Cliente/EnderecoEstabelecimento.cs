namespace Dashdine.Domain.Domain.Pedido.Cliente;

public sealed class EnderecoEstabelecimento(Guid id, string cep, string logradouro, int numero, string? complemento, string? bairro, string cidade, string estado, decimal latitude, decimal longitude, string timeZone)
{
    public Guid Id { get; } = id;
    public string CEP { get; } = cep;
    public string Logradouro { get; } = logradouro;
    public int Numero { get; } = numero;
    public string? Complemento { get; } = complemento;
    public string? Bairro { get; } = bairro;
    public string Cidade { get; } = cidade;
    public string Estado { get; } = estado;
    public decimal Latitude { get; } = latitude;
    public decimal Longitude { get; } = longitude;
    public string TimeZone { get; } = timeZone;

    public string Completo
    {
        get
        {
            return $"{Logradouro}, {Numero}{(string.IsNullOrEmpty(Complemento) ? string.Empty : $" - {Complemento}")}. {Cidade}, {Estado}.";
        }
    }
}
