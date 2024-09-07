using System.ComponentModel.DataAnnotations;

namespace Dashdine.Service.Models.Usuario;

public class DtoDeLogin
{
    [Required(ErrorMessage = "Obrigatório informar E-mail.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Obrigatório informar senha.")]
    public string Senha { get; set; }
}
