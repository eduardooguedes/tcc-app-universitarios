using Dashdine.Domain.Domain.Geolocalizacao;
using Dashdine.Domain.Domain.Integracoes.Google.Geocoding;
using Dashdine.Domain.Domain.Integracoes.Google.Timezone;
using Dashdine.Domain.Interface.Geolocalizacao;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Dashdine.Infrastructure.Repository.Geolocalizacao;

public class GeocodingGoogleRepository(IConfiguration configuration) : IGeolocalizacaoRepository
{
    private GoogleClient? googleClient;

    private const string MENSAGEM_SERVICO_INDISPONIVEL = "Geolocalização fora do ar. Tente novamente mais tarde.";
    private bool retentativaExcedida = false;

    private const string PAIS = "country";
    private const string ESTADO = "administrative_area_level_1";
    private const string CIDADE = "administrative_area_level_2";
    private readonly List<string> componenteBairro = new() { "sublocality_level_1", "sublocality" };
    private readonly List<string> componenteLogradouro = new() { "route", "street_address" };
    private const string NUMERO = "street_number";
    private const string CEP = "postal_code";

    public async Task<GeolocalizacaoDomain?> Obter(string logradouro, int numero, string? complemento, string cidade, string estado)
    {
        var enderecoCompleto = $"{logradouro}, {numero}{(string.IsNullOrEmpty(complemento) ? string.Empty : $" - {complemento}")}. {cidade}, {estado}.";

        if (string.IsNullOrEmpty(enderecoCompleto))
            return null;

        googleClient ??= ObterGoogleClient();

        var response = await googleClient.GetGeocodeAsync(enderecoCompleto);
        if (!response.IsSuccessStatusCode)
            return null;

        var resultObject = JsonConvert.DeserializeObject<GoogleGeocodingResult>(await response.Content.ReadAsStringAsync());
        if (resultObject is null)
            return null;

        if (resultObject.Status == "UNKNOWN_ERROR" && !retentativaExcedida)
        {
            retentativaExcedida = true;
            return await Obter(logradouro, numero, complemento, cidade, estado);
        }

        var domain = TratarERetornarDomain(resultObject);
        domain?.AdicionarTimeZone(await ObterTimeZone(domain.Latitude, domain.Longitude));
        return domain?.Latitude.HasValue == true && domain.Longitude.HasValue && !string.IsNullOrEmpty(domain.Timezone)
            ? domain
            : null;
    }

    public async Task<GeolocalizacaoDomain?> Obter(decimal latitude, decimal longitude)
    {
        googleClient ??= ObterGoogleClient();

        var response = await googleClient.GetGeocodeAsync($"{latitude},{longitude}");
        if (!response.IsSuccessStatusCode)
            return null;

        var resultObject = JsonConvert.DeserializeObject<GoogleGeocodingResult>(await response.Content.ReadAsStringAsync());
        if (resultObject is null)
            return null;

        if (resultObject.Status == "UNKNOWN_ERROR" && !retentativaExcedida)
        {
            retentativaExcedida = true;
            return await Obter(latitude, longitude);
        }

        var domain = TratarERetornarDomain(resultObject);
        domain?.AdicionarTimeZone(await ObterTimeZone(domain.Latitude, domain.Longitude));
        return domain?.Latitude.HasValue == true && domain.Longitude.HasValue && !string.IsNullOrEmpty(domain.Timezone)
            ? domain
            : null;
    }

    private GeolocalizacaoDomain? TratarERetornarDomain(GoogleGeocodingResult resultObject)
    {
        if (resultObject.Status == "OVER_QUERY_LIMIT")
            throw new ApplicationException(MENSAGEM_SERVICO_INDISPONIVEL);

        if (resultObject.Status != "OK")
            return null;

        if (!resultObject.Enderecos.Any())
            return null;

        var endereco = resultObject.Enderecos[0];
        if (endereco is null)
            return null;

        var codigoPais = endereco.ComponentesDoEndereco.Find(c => c.Tipos.Contains(PAIS))?.NomeResumido;
        var codigoEstado = endereco.ComponentesDoEndereco.Find(c => c.Tipos.Contains(ESTADO))?.NomeResumido;
        var cep = endereco.ComponentesDoEndereco.Find(c => c.Tipos.Contains(CEP))?.NomeResumido?.Replace("-", "");
        var logradouro = endereco.ComponentesDoEndereco.Find(c => c.Tipos.Contains(componenteLogradouro[0]) || c.Tipos.Contains(componenteLogradouro[1]))?.NomeCompleto;
        var bairro = endereco.ComponentesDoEndereco.Find(c => c.Tipos.Contains(componenteBairro[0]) || c.Tipos.Contains(componenteBairro[1]))?.NomeCompleto;
        var municipio = endereco.ComponentesDoEndereco.Find(c => c.Tipos.Contains(CIDADE))?.NomeCompleto;
        _ = int.TryParse(endereco.ComponentesDoEndereco.Find(c => c.Tipos.Contains(NUMERO))?.NomeCompleto, out int numero);

        decimal? latitude = null, longitude = null;
        if (endereco.Geometrias is not null)
        {
            latitude = endereco.Geometrias.Localizacao.Latitude;
            longitude = endereco.Geometrias.Localizacao.Longitude;
        }

        return new GeolocalizacaoDomain(logradouro, numero, cep, bairro, codigoPais, codigoEstado, municipio, latitude, longitude);
    }

    private async Task<string?> ObterTimeZone(decimal? latitude, decimal? longitude)
    {
        if (!latitude.HasValue || !longitude.HasValue)
            return string.Empty;

        googleClient ??= ObterGoogleClient();

        var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        var timezoneResponse = await googleClient.GetTimezoneAsync(latitude.Value, longitude.Value, timestamp);
        if (!timezoneResponse.IsSuccessStatusCode)
            return string.Empty;

        var resultObject = JsonConvert.DeserializeObject<GoogleTimezoneResult>(await timezoneResponse.Content.ReadAsStringAsync());
        if (resultObject is null)
            return string.Empty;

        if (!resultObject.Status.Equals("OK"))
            return string.Empty;

        return resultObject.TimeZoneId;
    }

    private GoogleClient ObterGoogleClient()
    {
        var geocode = configuration["geolocalizacao:google:geocode"]?.ToString() ?? throw new ApplicationException(MENSAGEM_SERVICO_INDISPONIVEL);
        var timezone = configuration["geolocalizacao:google:timezone"]?.ToString() ?? throw new ApplicationException(MENSAGEM_SERVICO_INDISPONIVEL);
        var baseUrl = configuration["geolocalizacao:google:baseUrl"]?.ToString() ?? throw new ApplicationException(MENSAGEM_SERVICO_INDISPONIVEL);
        var apiKey = configuration["geolocalizacao:google:apiKey"]?.ToString() ?? throw new ApplicationException(MENSAGEM_SERVICO_INDISPONIVEL);
        return new GoogleClient(baseUrl, apiKey, geocode, timezone);
    }
}
