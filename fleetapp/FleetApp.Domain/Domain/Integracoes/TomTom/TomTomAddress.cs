using Newtonsoft.Json;

namespace Dashdine.Domain.Domain.Integracoes.TomTom
{
    public class TomTomAddress
    {
        [JsonProperty("streetNumber")]
        public int Numero { get; set; }

        [JsonProperty("streetName")]
        public string Logradouro { get; set; }

        [JsonProperty("municipalitySubdivision")]
        public string Bairro { get; set; }

        //[JsonProperty("localName")]
        [JsonProperty("municipality")]
        public string Municipio { get; set; }

        [JsonProperty("countrySubdivisionCode")]
        public string CodigoEstado { get; set; }

        [JsonProperty("countrySubdivisionName")]
        public string NomeEstado { get; set; }

        [JsonProperty("extendedPostalCode")]
        public string CepComHifen { get; set; }

        public string Cep
        {
            get { return CepComHifen.Replace("-", string.Empty); }
        }

        [JsonProperty("countryCode")]
        public string CodigoPais { get; set; }

        [JsonProperty("freeformAddress")]
        public string EnderecoCompleto { get; set; }
    }
}
