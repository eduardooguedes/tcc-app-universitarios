namespace Dashdine.Service.Models.Cliente.Pedido;

public sealed record ProjecaoDeSituacaoDePagamento(int Id, string Descricao, bool Aguardando, bool EmAnalise, bool Pago, bool Cancelado, bool Rejeitado, bool Expirado);