namespace Dashdine.Application.Controllers.Shared;

public class RetornoPadrao
{
    public object? Data { get; }
    public string? Erro { get; }

    public RetornoPadrao(string erro)
    {
        Data = null;
        Erro = erro;
    }

    public RetornoPadrao(object? data, string? erro = null)
    {
        Data = data;
        Erro = erro;
    }
}