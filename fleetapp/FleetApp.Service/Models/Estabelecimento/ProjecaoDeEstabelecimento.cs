using Dashdine.Domain.Domain.Estabelecimento;

namespace Dashdine.Service.Models.Estabelecimento;

public class ProjecaoDeEstabelecimento
{
    public string Id { get; set; }
    public SituacaoDeEstabelecimento Situacao { get; set; }
    public string? Logo { get; set; }
    public string NomeFantasia { get; set; }
    public string RazaoSocial { get; set; }
    public string? Telefone { get; set; }
    public DateTime DataHoraCadastro { get; set; }
    public ProjecaoDeEnderecoEstabelecimento? EnderecoRetirada { get; set; }
}
