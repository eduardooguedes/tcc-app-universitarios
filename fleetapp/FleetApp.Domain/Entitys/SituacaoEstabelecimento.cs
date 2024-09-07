using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class SituacaoEstabelecimento
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public bool Ativo { get; set; }

    public bool Novo { get; set; }

    public virtual ICollection<Estabelecimento> Estabelecimentos { get; set; } = new List<Estabelecimento>();
}
