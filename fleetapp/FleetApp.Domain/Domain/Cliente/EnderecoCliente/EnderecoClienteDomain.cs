namespace Dashdine.Domain.Domain.Cliente.EnderecoCliente;

public sealed record EnderecoClienteDomain(Guid Id, Guid IdCliente, TipoEnderecoClienteDomain TipoDeEndereco, string Apelido, bool Principal, string Cep, string Logradouro, int Numero, string? Complemento, string? Bairro, string Cidade, string Estado, decimal Latitude, decimal Longitude, string Timezone)
{
    public string DescricaoCompleta
    {
        get
        {
            return $"{Logradouro}, {Numero}{(string.IsNullOrEmpty(Complemento) ? string.Empty : $" - {Complemento}")}. {Cidade}, {Estado}.";
        }
    }
}
