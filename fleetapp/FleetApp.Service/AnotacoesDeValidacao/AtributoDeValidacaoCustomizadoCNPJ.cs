using Dashdine.Service.Validacoes;
using System.ComponentModel.DataAnnotations;

namespace Dashdine.Service.AnotacoesDeValidacao;

public class AtributoDeValidacaoCustomizadoCNPJ : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null)
            return false;

        if (string.IsNullOrEmpty(value.ToString()))
            return false;

        if (value is not string)
            return false;

        return ValidacaoCNPJ.ValidarCNPJ((string)value);
    }
}
