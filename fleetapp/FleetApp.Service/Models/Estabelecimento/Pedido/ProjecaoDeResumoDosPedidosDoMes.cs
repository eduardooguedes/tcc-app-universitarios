namespace Dashdine.Service.Models.Estabelecimento.Pedido;

public sealed record ProjecaoDeResumoDosPedidosDoMes(DateOnly MesEAno, decimal TotalVendido, int QuantidadeDePedidosRetirados, int QuantidadeDePedidosNovos, IEnumerable<ProjecaoDeDiaDoResumoDosPedidosDoMes> Dias);
