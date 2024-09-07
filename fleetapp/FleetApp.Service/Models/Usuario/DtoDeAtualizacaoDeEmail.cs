using System.ComponentModel.DataAnnotations;

namespace Dashdine.Service.Models.Usuario;

public class DtoDeAtualizacaoDeEmail
{
    /// <summary>
    /// 
    /// </summary>
    [Required(ErrorMessage = "Informe o novo email.")]
    public string NovoEmail { get; set; }
}
