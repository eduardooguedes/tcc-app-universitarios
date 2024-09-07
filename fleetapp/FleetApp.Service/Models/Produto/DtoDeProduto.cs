using System.ComponentModel.DataAnnotations;

namespace Dashdine.Service.Models.Produto;

public class DtoDeProduto
{
    public int IdCategoria { get; set; }
    public int IdTipoProduto { get; set; }

    [Required(ErrorMessage = "Informe um nome para o produto.")]
    [StringLength(25, ErrorMessage = "Informe um nome para o produto em até 25 caracteres.")]
    public string Nome { get; set; } = "";

    [StringLength(300, ErrorMessage = "Informe uma descrição para o produto em até 300 caracteres.")]
    public string Descricao { get; set; } = "";
    public decimal Preco { get; set; } 
    public int TempoEmMinutosParaRetirada { get; set; }

    public List<DtoDeAdicionalDoProduto>? Adicionais { get; set; }
}
