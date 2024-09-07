using System.ComponentModel.DataAnnotations;

namespace Dashdine.Service.Models.Cliente;

public class DtoDoTitularDoCartao
{
    [Required(ErrorMessage = "Obrigatório informar celular.")]
    [StringLength(30, ErrorMessage = "Informe um nome para o titular que possua até 40 caracteres.")]
    public string NomeDoTitular { get; set; }

    [Required(ErrorMessage = "Obrigatório informar CPF ou CNPJ para o titular.")]
    [StringLength(14, MinimumLength = 11, ErrorMessage = "Informe um cpf ou cnpj com tamanho válido para o titular.")]
    public string CpfOuCnpjDoTitular { get; set; }
}
