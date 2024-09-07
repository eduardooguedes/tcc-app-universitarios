namespace Dashdine.Domain.Domain.Pedido.Estabelecimento;

public sealed class ResumoDosPedidosDoMesDomain
{
    public DateOnly MesEAno { get; }
    public decimal TotalVendido { get; }
    public int QuantidadeDePedidosRetirados { get; }
    public int QuantidadeDePedidosNovos { get; }
    public IEnumerable<DiaDoResumoDosPedidosDoMesDomain> Dias { get; }

    public ResumoDosPedidosDoMesDomain(DateOnly mesEAno, decimal totalVendido, int quantidadeDePedidosRetirados, int quantidadeDePedidosNovos, IEnumerable<DiaDoResumoDosPedidosDoMesDomain> dias)
    {
        MesEAno = mesEAno;
        TotalVendido = totalVendido;
        QuantidadeDePedidosRetirados = quantidadeDePedidosRetirados;
        QuantidadeDePedidosNovos = quantidadeDePedidosNovos;
        Dias = dias;
    }
}
