using System.ComponentModel.DataAnnotations;

namespace Dashdine.Service.Models.Estabelecimento;

public class DtoDeEstabelecimentoEdicao
{
    [Required(ErrorMessage = "Obrigatório informar nome fantasia.")]
    [StringLength(30, ErrorMessage = "Informe um nome fantasia em até 30 caracteres.")]
    public string NomeFantasia { get; set; }

    [Required(ErrorMessage = "Obrigatório informar razão social.")]
    [StringLength(50, ErrorMessage = "Informe uma razão social em até 50 caracteres.")]
    public string RazaoSocial { get; set; }

    [Required(ErrorMessage = "Obrigatório informar telefone.")]
    [StringLength(13, MinimumLength = 10, ErrorMessage = "Informe um número de telefone que possua de 10 a 13 caracteres.")]
    public string Telefone { get; set; }
}
