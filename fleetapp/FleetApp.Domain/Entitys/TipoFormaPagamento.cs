using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class TipoFormaPagamento
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<FormasComunsPagamento> FormasComunsPagamentos { get; set; } = new List<FormasComunsPagamento>();
}
