using Dashdine.Domain.Domain.Estabelecimento.HorarioDeFuncionamento;

namespace Dashdine.Domain.Domain.Cliente.Estabelecimento;

public sealed class HorarioDeFuncionamentoDoDiaParaClienteDomain
{
    public TimeOnly Horario { get; }
    public IEnumerable<DestinoDaRetiradaDoPedidoDomain> Destinos { get; }

    public HorarioDeFuncionamentoDoDiaParaClienteDomain(TimeOnly horario, IEnumerable<DestinoDaRetiradaDoPedidoDomain> destinos)
    {
        Horario = horario;
        Destinos = destinos;
    }
}
