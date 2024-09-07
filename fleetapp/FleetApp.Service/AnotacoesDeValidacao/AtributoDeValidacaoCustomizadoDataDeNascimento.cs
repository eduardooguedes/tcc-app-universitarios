using Dashdine.Service.Validacoes;
using System.ComponentModel.DataAnnotations;

namespace Dashdine.Service.AnotacoesDeValidacao
{

    public class AtributoDeValidacaoCustomizadoDataDeNascimento : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is not DateOnly)
                return false;

            return ValidacaoDataDeNascimento.ValidaDataDeNascimento((DateOnly)value);
        }
    }
}
