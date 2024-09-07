using Dashdine.Service.AnotacoesDeValidacao;
using System.ComponentModel.DataAnnotations;

namespace Dashdine.Service.Models.Estabelecimento.Gestor;

public class DtoDeGestor
{
    [Required(ErrorMessage = "Obrigatório informar nome.")]
    [StringLength(25, ErrorMessage = "Informe um nome em até 25 caracteres.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Obrigatório informar sobrenome.")]
    [StringLength(30, ErrorMessage = "Informe um sobrenome em até 30 caracteres.")]
    public string Sobrenome { get; set; }

    [Required(ErrorMessage = "Obrigatório informar data de nascimento.")]
    [AtributoDeValidacaoCustomizadoDataDeNascimento(ErrorMessage = "Data de nascimento inválida.")]
    public DateOnly DataNascimento { get; set; }

    [Required(ErrorMessage = "Obrigatório informar CPF.")]
    [AtributoDeValidacaoCustomizadoCPF(ErrorMessage = "CPF inválido.")]
    public string CPF { get; set; }

    [Required(ErrorMessage = "Obrigatório informar email.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    [StringLength(50, ErrorMessage = "Informe um e-mail que possua até 50 caracteres.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Obrigatório informar senha.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Informe uma senha que possua no mínimo 8 caracteres.")]
    public string Senha { get; set; }
}
