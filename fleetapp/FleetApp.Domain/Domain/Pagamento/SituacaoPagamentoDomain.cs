namespace Dashdine.Domain.Domain.Pagamento;

public sealed record SituacaoPagamentoDomain(int Id, string Descricao, bool Aguardando, bool EmAnalise, bool Pago, bool Cancelado, bool Rejeitado, bool Expirado, bool EstornarAoCancelarPedido);
