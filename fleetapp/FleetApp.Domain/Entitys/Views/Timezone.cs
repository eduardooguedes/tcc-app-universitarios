using System.ComponentModel.DataAnnotations.Schema;

namespace Dashdine.Domain.Entitys.Views;

public sealed class Timezone
{
    [Column("data_hora_atual")]
    public DateTime DataHoraAtual { get; set; }
}
