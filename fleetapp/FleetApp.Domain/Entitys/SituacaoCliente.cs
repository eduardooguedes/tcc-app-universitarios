using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class SituacaoCliente
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
}
