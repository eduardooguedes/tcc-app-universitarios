namespace Dashdine.Domain.Domain.Estabelecimento;

public sealed record EnderecoRetirada(Guid Id, string Cep, string? Bairro, string Estado, string Cidade, string Logradouro, int Numero, string? Complemento, decimal Latitude, decimal Longitude, string TimeZone)
{
    public string Completo
    {
        get
        {
            return $"{Logradouro}, {Numero}{(string.IsNullOrEmpty(Complemento) ? string.Empty : $" - {Complemento}")}. {Cidade}, {Estado}.";
        }
    }
}
