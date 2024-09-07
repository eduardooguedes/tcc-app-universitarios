using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class AdicionalProduto
{
    public Guid IdAdicional { get; set; }

    public Guid IdProduto { get; set; }

    public int? QtdeMaxima { get; set; }

    public virtual Adicional IdAdicionalNavigation { get; set; } = null!;

    public virtual Produto IdProdutoNavigation { get; set; } = null!;
}
