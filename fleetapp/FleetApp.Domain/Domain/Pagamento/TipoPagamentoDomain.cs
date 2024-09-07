namespace Dashdine.Domain.Domain.Pagamento;

public sealed record TipoPagamentoDomain(int Id, string Descricao, bool CartaoCredito, bool Pix);