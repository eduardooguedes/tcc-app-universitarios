namespace Dashdine.Domain.Domain.Estabelecimento;

public sealed record EstabelecimentoDomain(Guid Id, SituacaoDeEstabelecimento Situacao, DateTime DataHoraCadastro, string NomeFantasia, string RazaoSocial, string CNPJ, string? Telefone, string? Logo, EnderecoRetirada? Endereco);