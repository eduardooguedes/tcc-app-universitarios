
using System.ComponentModel.DataAnnotations;

namespace Dashdine.Service.Models.Cliente;

public class DtoDeEnderecoCliente
{
    /// <summary>
    /// Tipo de endereço do cliente: 1 - Retirada | 2 - Pagamento
    /// </summary>
    [Required(ErrorMessage = "Obrigatório informar tipo de endereço")]
    [Range(1.0, 2.0, ErrorMessage = "Defina o tipo de endereço: 1 - Retirada | 2 - Pagamento ")]
    public int Tipo { get; set; }

    [Required(ErrorMessage = "Obrigatório informar Apelido.")]
    [StringLength(20, ErrorMessage = "Informe Apelido em até 20 caracteres.")]
    public string Apelido { get; set; }

    /// <summary>
    /// Determina se endereço é o principal dos endereços que possuam mesmo tipo.
    /// </summary>
    public bool Principal { get; set; } = false;

    [Required(ErrorMessage = "Obrigatório informar Estado.")]
    [StringLength(2, ErrorMessage = "Informe código do Estado em até 2 caracteres.")]
    public string Estado { get; set; }

    [Required(ErrorMessage = "Obrigatório informar Cidade.")]
    [StringLength(28, ErrorMessage = "Informe Cidade em até 28 caracteres.")]
    public string Cidade { get; set; }

    [Required(ErrorMessage = "Obrigatório informar CEP.")]
    [StringLength(8, ErrorMessage = "Informe CEP em até 8 caracteres.")]
    public string CEP { get; set; }

    [Required(ErrorMessage = "Obrigatório informar Logradouro.")]
    [StringLength(45, ErrorMessage = "Informe Logradouro em até 45 caracteres.")]
    public string Logradouro { get; set; }

    [Required(ErrorMessage = "Obrigatório informar bairro.")]
    [StringLength(15, ErrorMessage = "Informe bairro em até 15 caracteres.")]
    public string Bairro { get; set; }

    [Required(ErrorMessage = "Obrigatório informar Número.")]
    public int Numero { get; set; }

    [StringLength(15, ErrorMessage = "Informe Complemento em até 15 caracteres.")]
    public string? Complemento { get; set; }
}
