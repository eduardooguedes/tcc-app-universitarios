namespace Dashdine.Domain.Domain.Estabelecimento.HorarioDeFuncionamento;

public sealed class DiaDaSemana(int id, string descricao)
{
    public int Id { get; } = id;
    public string Descricao { get; } = descricao;

    public override bool Equals(object? obj)
    {
        if (obj is not DiaDaSemana)
            return false;

        return Id.Equals(((DiaDaSemana)obj).Id);
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static List<DiaDaSemana> Lista
    {
        get
        {
            List<DiaDaSemana> lista =
            [
                Domingo,
                Segunda,
                Terca,
                Quarta,
                Quinta,
                Sexta,
                Sabado
            ];
            return [.. lista];
        }
    }

    public static DiaDaSemana ObterDiaAtual() => Lista.First(d => d.Id == (int)DateTime.Now.DayOfWeek);

    public static DiaDaSemana Domingo { get { return new DiaDaSemana(0, "Domingo"); } }
    public static DiaDaSemana Segunda { get { return new DiaDaSemana(1, "Segunda"); } }
    public static DiaDaSemana Terca { get { return new DiaDaSemana(2, "Terça"); } }
    public static DiaDaSemana Quarta { get { return new DiaDaSemana(3, "Quarta"); } }
    public static DiaDaSemana Quinta { get { return new DiaDaSemana(4, "Quinta"); } }
    public static DiaDaSemana Sexta { get { return new DiaDaSemana(5, "Sexta"); } }
    public static DiaDaSemana Sabado { get { return new DiaDaSemana(6, "Sabado"); } }
}