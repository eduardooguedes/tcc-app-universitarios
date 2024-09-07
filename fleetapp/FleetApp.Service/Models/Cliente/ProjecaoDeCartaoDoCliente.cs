namespace Dashdine.Service.Models.Cliente;

public sealed record ProjecaoDeCartaoDoCliente(Guid Id, string? Imagem, string? Apelido, string FinalDoNumero, string Tipo, DateOnly Validade, string Bandeira);