using Dashdine.Service.AnotacoesDeValidacao;
using System.ComponentModel.DataAnnotations;

namespace Dashdine.Service.Models.Estabelecimento.Gestor;

public class DtoDeEdicaoDeGestor
{
    [Required(ErrorMessage = "Obrigatório informar nome.")]
    [StringLength(25, ErrorMessage = "Informe um nome em até 25 caracteres.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Obrigatório informar sobrenome.")]
    [StringLength(30, ErrorMessage = "Informe um sobrenome em até 30 caracteres.")]
    public string Sobrenome { get; set; }

    [Required(ErrorMessage = "Obrigatório informar data de nascimento.")]
    [AtributoDeValidacaoCustomizadoDataDeNascimento(ErrorMessage = "Data de nascimento inválida.")]
    public DateOnly? DataNascimento { get; set; }
}
