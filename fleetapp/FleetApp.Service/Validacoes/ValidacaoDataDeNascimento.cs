namespace Dashdine.Service.Validacoes;

public static class ValidacaoDataDeNascimento
{
    public static bool ValidaDataDeNascimento(DateOnly data)
    {
        if (data.AddYears(0) > DateOnly.FromDateTime(DateTime.Now))
        {
            return false;
        }
        return true;
    }
}
