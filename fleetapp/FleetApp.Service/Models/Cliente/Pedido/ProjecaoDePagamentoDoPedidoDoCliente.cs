namespace Dashdine.Service.Models.Cliente.Pedido;

public sealed record ProjecaoDePagamentoDoPedidoDoCliente(Guid? IdFormaDePagamento, int TipoDaFormaDePagamento, ProjecaoDeSituacaoDePagamento SituacaoPagamento, int? TempoRestanteEmSegundos, string? LinkParaPagamento, string? Imagem);