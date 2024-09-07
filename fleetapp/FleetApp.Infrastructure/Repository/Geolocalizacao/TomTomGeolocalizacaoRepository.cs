using Dashdine.Domain.Domain.Geolocalizacao;
using Dashdine.Domain.Domain.Integracoes.TomTom;
using Dashdine.Domain.Interface.Geolocalizacao;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Dashdine.Infrastructure.Repository.Geolocalizacao;

public class TomTomGeolocalizacaoRepository : IGeolocalizacaoRepository
{
    private readonly IConfiguration configuration;
    private const string MENSAGEM_SERVICO_INDISPONIVEL = "Serviço indisponível no momento.";
    private const string CONTENT_TYPE = ".json";

    public TomTomGeolocalizacaoRepository(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    /// <summary>
    /// https://developer.tomtom.com/geocoding-api/api-explorer
    /// </summary>
    /// <param name="cep"></param>
    /// <param name="numero"></param>
    public async Task<GeolocalizacaoDomain?> Obter(string logradouro, int numero, string? complemento, string cidade, string estado)
    {
        var enderecoCompleto = $"{logradouro}, {numero}{(string.IsNullOrEmpty(complemento) ? string.Empty : $" - {complemento}")}. {cidade}, {estado}."; 

        var baseUrl = configuration["geolocalizacao:tomtom:baseUrl"]?.ToString() ?? throw new Exception(MENSAGEM_SERVICO_INDISPONIVEL);
        var versao = configuration["geolocalizacao:tomtom:version"]?.ToString() ?? throw new Exception(MENSAGEM_SERVICO_INDISPONIVEL);
        var countrySet = configuration["geolocalizacao:tomtom:countrySet"]?.ToString() ?? throw new Exception(MENSAGEM_SERVICO_INDISPONIVEL);
        var key = configuration["geolocalizacao:tomtom:apiKey"]?.ToString() ?? throw new Exception(MENSAGEM_SERVICO_INDISPONIVEL);

        var httpClient = ObterHttpClient(baseUrl);

        string query = $"{enderecoCompleto}";
        var response = await httpClient.GetAsync($"search/{versao}/geocode/{query}{CONTENT_TYPE}?storeResult=false&countrySet={countrySet}&view=Unified&key={key}");
        if (!response.IsSuccessStatusCode)
            return null;

        var resultObject = JsonConvert.DeserializeObject<TomTomGeocodingApiResponse>(await response.Content.ReadAsStringAsync());
        if (resultObject is null)
            return null;

        if (!resultObject.Results.Any())
            return null;

        var endereco = resultObject.Results[0].Address;
        if (endereco is null)
            return null;

        var posicaoGeografica = resultObject.Results[0].Position;
        if (posicaoGeografica is null)
            return null;

        return new GeolocalizacaoDomain(endereco.Logradouro, endereco.Numero, endereco.Cep, endereco.Bairro, endereco.CodigoPais, endereco.CodigoEstado, endereco.Municipio, posicaoGeografica.Latitude, posicaoGeografica.Longitude);
    }

    /// <summary>
    /// https://developer.tomtom.com/reverse-geocoding-api/api-explorer
    /// </summary>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    public async Task<GeolocalizacaoDomain?> Obter(decimal latitude, decimal longitude)
    {
        var baseUrl = configuration["geolocalizacao:tomtom:baseUrl"]?.ToString() ?? throw new Exception(MENSAGEM_SERVICO_INDISPONIVEL);
        var versao = configuration["geolocalizacao:tomtom:version"]?.ToString() ?? throw new Exception(MENSAGEM_SERVICO_INDISPONIVEL);
        var key = configuration["geolocalizacao:tomtom:apiKey"]?.ToString() ?? throw new Exception(MENSAGEM_SERVICO_INDISPONIVEL);
        var httpClient = ObterHttpClient(baseUrl);

        var position = $"{latitude}%2C{longitude}";
        var response = await httpClient.GetAsync($"search/{versao}/reverseGeocode/{position}{CONTENT_TYPE}?radius=1000&view=Unified&key={key}");
        if (!response.IsSuccessStatusCode)
            return null;

        var resultObject = JsonConvert.DeserializeObject<TomTomReverseGeocodingApiResponse>(await response.Content.ReadAsStringAsync());
        if (resultObject is null)
            return null;

        if (!resultObject.Addresses.Any())
            return null;

        var endereco = resultObject.Addresses[0].Address;
        if (endereco is null)
            return null;

        var posicaoGeografica = resultObject.Addresses[0].Position;
        if (posicaoGeografica is null)
            return null;

        return new GeolocalizacaoDomain(endereco.Logradouro, endereco.Numero, endereco.Cep, endereco.Bairro, endereco.CodigoPais, endereco.CodigoEstado, endereco.Municipio, posicaoGeografica.Latitude, posicaoGeografica.Longitude);
    }

    private HttpClient ObterHttpClient(string urlBase) =>
        new()
        {
            Timeout = TimeSpan.FromSeconds(30),
            BaseAddress = new Uri(urlBase),
        };
}
