using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class SituacaoGestor
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<Gestor> Gestors { get; set; } = new List<Gestor>();
}
