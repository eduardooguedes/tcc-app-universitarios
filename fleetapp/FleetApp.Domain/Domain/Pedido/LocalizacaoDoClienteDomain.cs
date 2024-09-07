namespace Dashdine.Domain.Domain.Pedido;

public sealed record LocalizacaoDoClienteDomain(Guid Id, string Apelido, decimal Latitude, decimal Longitude, string Timezone);