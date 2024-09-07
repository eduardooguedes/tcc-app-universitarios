namespace Dashdine.Infrastructure.Repository.Geolocalizacao;

public class GoogleClient : HttpClient
{
    private readonly string apiKey;
    private readonly string geocode;
    private readonly string timezone;
    private const string OUTPUT_FORMAT = "json";

    public GoogleClient(string urlBase, string apiKey, string geocode, string timezone)
    {
        Timeout = TimeSpan.FromSeconds(20);
        BaseAddress = new Uri(urlBase);
        this.apiKey = apiKey;
        this.geocode = geocode;
        this.timezone = timezone;
    }

    public async Task<HttpResponseMessage> GetGeocodeAsync(string queryAddress) => await GetAsync($"{geocode}/{OUTPUT_FORMAT}?address={queryAddress}&key={apiKey}");
    public async Task<HttpResponseMessage> GetTimezoneAsync(decimal latitude, decimal longitude, long timestamp) => await GetAsync($"{timezone}/{OUTPUT_FORMAT}?location={latitude}%2C{longitude}&timestamp={timestamp}&key={apiKey}");
}
