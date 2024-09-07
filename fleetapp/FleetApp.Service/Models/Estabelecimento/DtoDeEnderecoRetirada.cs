using System.ComponentModel.DataAnnotations;

namespace Dashdine.Service.Models.Estabelecimento;

public class DtoDeEnderecoRetirada
{
    [Required(ErrorMessage = "Obrigatório informar CEP.")]
    [StringLength(8, ErrorMessage = "Informe CEP em até 8 caracteres.")]
    public string CEP { get; set; }

    [Required(ErrorMessage = "Obrigatório informar Número.")]
    public int Numero { get; set; }

    [Required(ErrorMessage = "Obrigatório informar Logradouro.")]
    [StringLength(45, ErrorMessage = "Informe Logradouro em até 45 caracteres.")]
    public string Logradouro { get; set; }

    [StringLength(15, ErrorMessage = "Informe Complemento em até 15 caracteres.")]
    public string? Complemento { get; set; }

    [Required(ErrorMessage = "Obrigatório informar bairro.")]
    [StringLength(15, ErrorMessage = "Informe bairro em até 15 caracteres.")]
    public string Bairro { get; set; }

    [Required(ErrorMessage = "Obrigatório informar Estado.")]
    [StringLength(2, ErrorMessage = "Informe código do Estado em até 2 caracteres.")]
    public string Estado { get; set; }

    [Required(ErrorMessage = "Obrigatório informar Cidade.")]
    [StringLength(28, ErrorMessage = "Informe Cidade em até 28 caracteres.")]
    public string Cidade { get; set; }
}
