using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class SituacaoHorariosFuncionamento
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public bool Ativo { get; set; }

    public virtual ICollection<HorariosFuncionamento> HorariosFuncionamentos { get; set; } = new List<HorariosFuncionamento>();
}
