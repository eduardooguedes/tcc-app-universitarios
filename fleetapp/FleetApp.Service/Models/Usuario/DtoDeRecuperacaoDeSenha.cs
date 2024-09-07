using System.ComponentModel.DataAnnotations;

namespace Dashdine.Service.Models.Usuario;

public class DtoDeRecuperacaoDeSenha
{
    [Required(ErrorMessage = "Informe sua nova senha.")]
    [StringLength(50, MinimumLength = 8, ErrorMessage = "Informe no mínimo 8 caracteres para sua nova senha.")]
    public string SenhaNova { get; set; }
}
