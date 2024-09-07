namespace Dashdine.Service.Models.Cliente;

public sealed record ProjecaoDeCliente(
    string Id, int Situacao, string CPF, string Nome, string Sobrenome, string Email, bool EmailConfirmado, string Celular, bool CelularConfirmado, DateTime DataDeCadastro, DateOnly DataDeNascimento, List<ProjecaoDeEnderecoCliente>? Enderecos);
