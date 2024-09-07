namespace Dashdine.Domain.Domain.Pedido.Estabelecimento;

public sealed class DiaDoResumoDosPedidosDoMesDomain
{
    public int DiaDoMes { get; }
    public int DiaDaSemana { get; }
    public int QuantidadeDePedidosNovos { get; }

    public DiaDoResumoDosPedidosDoMesDomain(int diaDoMes, int diaDaSemana, int quantidadeDePedidosNovos)
    {
        DiaDoMes = diaDoMes;
        DiaDaSemana = diaDaSemana;
        QuantidadeDePedidosNovos = quantidadeDePedidosNovos;
    }
}
