namespace Dashdine.Domain.Filtros;

public sealed class FiltrosDomain
{
    public int Pagina { get; }
    public int QuantidadeDeRegistros { get; }

    public FiltrosDomain(int pagina, int quantidadeDeRegistros)
    {
        Pagina = pagina;
        QuantidadeDeRegistros = quantidadeDeRegistros;
    }
}
