using System.ComponentModel.DataAnnotations.Schema;

namespace Dashdine.Domain.Entitys.Views;

public class EstabelecimentoProximoView
{
    [Column("id")]
    public Guid Id { get; set; }

    [Column("nomefantasia")]
    public string NomeFantasia { get; set; }

    [Column("logo")]
    public string? Logo { get; set; }

    [Column("distanciaemmetros")]
    public int DistanciaEmMetros { get; set; }

    [Column("proximohorariohoje")]
    public TimeOnly? ProximoHorarioHoje { get; set; }

    [Column("proximohorarioamanha")]
    public TimeOnly? ProximoHorarioAmanha { get; set; }
}
