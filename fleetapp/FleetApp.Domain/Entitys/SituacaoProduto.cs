using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class SituacaoProduto
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public bool Ativo { get; set; }

    public virtual ICollection<Adicional> Adicionals { get; set; } = new List<Adicional>();

    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
