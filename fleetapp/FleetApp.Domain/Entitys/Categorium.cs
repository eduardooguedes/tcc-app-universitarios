using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class Categorium
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
