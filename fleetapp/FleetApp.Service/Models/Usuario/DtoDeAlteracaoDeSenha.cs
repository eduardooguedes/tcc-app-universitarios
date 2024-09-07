using System.ComponentModel.DataAnnotations;

namespace Dashdine.Service.Models.Usuario;

public class DtoDeAlteracaoDeSenha : DtoDeRecuperacaoDeSenha
{
    [Required(ErrorMessage = "Informe senha atual.")]
    public string SenhaAtual { get; set; }
}
