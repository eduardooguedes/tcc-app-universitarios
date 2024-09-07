namespace Dashdine.Domain.Domain.Estabelecimento.HorarioDeFuncionamento;

public sealed class DestinoDaRetiradaDoPedidoDomain
{
    public int Id { get; }
    public string Descricao { get; }

    public DestinoDaRetiradaDoPedidoDomain(int id, string descricao)
    {
        Id = id;
        Descricao = descricao;
    }

    private static List<DestinoDaRetiradaDoPedidoDomain> Lista
    {
        get
        {
            return
            [
                ParaLevar,
                ComerNoLocal
            ];
        }
    }

    public static DestinoDaRetiradaDoPedidoDomain? ObterPorId(int destinoDaRetirada) => Lista.Find(d => d.Id == destinoDaRetirada);

    public static DestinoDaRetiradaDoPedidoDomain ParaLevar { get { return new DestinoDaRetiradaDoPedidoDomain(1, "Para levar"); } private set { } }
    public static DestinoDaRetiradaDoPedidoDomain ComerNoLocal { get { return new DestinoDaRetiradaDoPedidoDomain(2, "Comer no local"); } private set { } }
}
