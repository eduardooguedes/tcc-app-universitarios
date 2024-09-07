namespace Dashdine.Domain.Domain.Cliente.Estabelecimento;

public sealed class HorarioDeFuncionamentoParaClienteDomain
{
    public DateOnly Dia { get; }
    public IEnumerable<HorarioDeFuncionamentoDoDiaParaClienteDomain> Horarios { get; }

    public HorarioDeFuncionamentoParaClienteDomain(DateOnly dia, IEnumerable<HorarioDeFuncionamentoDoDiaParaClienteDomain> horarios)
    {
        Dia = dia;
        Horarios = horarios;
    }
}
