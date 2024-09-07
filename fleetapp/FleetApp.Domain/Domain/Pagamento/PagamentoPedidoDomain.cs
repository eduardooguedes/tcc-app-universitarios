namespace Dashdine.Domain.Domain.Pagamento;

public sealed record PagamentoPedidoDomain(Guid Id, SituacaoPagamentoDomain Situacao, TipoPagamentoDomain Tipo, decimal Valor, DateTime DataHora, DateTime? DataHoraAtualizado, DateTime? DataHoraExpiracao, Guid? IdCartao, string? LinkPagamento, string? ImagemQrCode)
{
    public int ObterSegundosParaExpirar(DateTime dataHoraAtual) => (int)TimeSpan.FromTicks(DataHoraExpiracao is null ? 0 : DataHoraExpiracao!.Value.Ticks - dataHoraAtual.Ticks).TotalSeconds;
}
