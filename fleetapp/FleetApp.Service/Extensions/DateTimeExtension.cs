using System.Globalization;

namespace Dashdine.Service.Extensions;

public static class DateTimeExtension
{
    /// <summary>
    /// Exemplos:
    /// "Quinta, 25/01/2023".
    /// "Quinta, 25/01".
    /// </summary>
    /// <param name="dateTime"></param>
    public static string? ToDiaDaSemanaEDiaMes(this DateTime? dateTime) =>
        dateTime?
        .ToString($"dddd, dd/MM{(dateTime.Value.Year == DateTime.Now.Year ? string.Empty : "/yyyy")}", CultureInfo.CreateSpecificCulture("pt-BR"))
        .Replace("-feira", string.Empty)
        .ToFirstUpper();

    public static TimeOnly? ToTimeOnly(this DateTime? dateTime) => dateTime.HasValue ? TimeOnly.FromDateTime(dateTime.Value) : null;
}
