using System.ComponentModel.DataAnnotations;

namespace Dashdine.Service.Models.Cliente;

public class DtoDeCartaoDoClienteEdicao
{
    [StringLength(20, ErrorMessage = "Informe um apelido em até 20 caracteres.")]
    public string Apelido { get; set; }
}
