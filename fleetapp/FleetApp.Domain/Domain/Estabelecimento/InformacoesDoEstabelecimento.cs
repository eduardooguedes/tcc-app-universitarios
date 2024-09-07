using Dashdine.Domain.Domain.Produto;

namespace Dashdine.Domain.Domain.Estabelecimento;

public sealed class InformacoesDoEstabelecimento
{
    public Guid Id { get; set; }
    public string NomeFantasia { get; set; }

    /// <summary>
    /// Formato: {Logradouro}, {Número} - {Complemento}. {Cidade}, {Estado}.
    /// </summary>
    public string EnderecoCompleto { get; set; }

    public InformacoesSobreOsProdutos InformacoesSobreOsProdutos { get; set; }
}
