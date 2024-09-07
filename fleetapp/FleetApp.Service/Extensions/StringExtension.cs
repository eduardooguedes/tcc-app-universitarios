namespace Dashdine.Service.Extensions;

public static class StringExtension
{
    public static string? ToFirstUpper(this string? texto) => string.IsNullOrEmpty(texto) ? null : string.Concat(texto[0].ToString().ToUpper(), texto.Length > 1 ? texto[1..] : string.Empty);
}
