using Dashdine.Domain.Domain.Produto;

namespace Dashdine.Service.Models.Estabelecimento;

public class ProjecaoDeInformacoesDoEstabelecimento
{
    public string Id { get; set; }
    public string NomeFantasia { get; set; }

    /// <summary>
    /// Formato: {Logradouro}, {Número} - {Complemento}. {Cidade}, {Estado}.
    /// </summary>
    public string EnderecoCompleto { get; set; }

    public InformacoesSobreOsProdutos InformacoesSobreOsProdutos { get; set; }
}
                                                                                                                                                                                   