namespace Dashdine.Domain.Domain.Cliente.Cartao;

public sealed record CartaoDomain(Guid Id, Guid IdCliente, string? Apelido, string FinalDoNumero, EnumTipoDeCartao Tipo, DateOnly Validade, string Bandeira, Guid IdEnderecoCobranca);