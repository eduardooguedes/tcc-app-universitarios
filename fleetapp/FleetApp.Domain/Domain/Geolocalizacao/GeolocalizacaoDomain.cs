namespace Dashdine.Domain.Domain.Geolocalizacao;

public sealed class GeolocalizacaoDomain
{
    public string? Logradouro { get; }
    public int? Numero { get; }
    public string? Cep { get; }
    public string? Bairro { get; }
    public string? CodigoPais { get; }
    public string? CodigoEstado { get; }
    public string? Municipio { get; }
    public decimal? Latitude { get; }
    public decimal? Longitude { get; }
    public string? Timezone { get; private set; }

    public GeolocalizacaoDomain(string? logradouro, int? numero, string? cep, string? bairro, string? codigoPais, string? codigoEstado, string? municipio, decimal? latitude, decimal? longitude)
    {
        Logradouro = logradouro;
        Numero = numero;
        Cep = cep;
        Bairro = bairro;
        CodigoPais = codigoPais;
        CodigoEstado = codigoEstado;
        Municipio = municipio;
        Latitude = latitude;
        Longitude = longitude;
    }

    public void AdicionarTimeZone(string? timezone) => Timezone = timezone;
}
