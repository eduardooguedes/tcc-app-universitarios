using System.ComponentModel.DataAnnotations;

namespace Dashdine.Service.Models.Produto;

public class DtoDeAdicional
{
    [StringLength(20, MinimumLength = 3)]
    public string Nome { get; set; }
    public decimal Preco { get; set; }
}
