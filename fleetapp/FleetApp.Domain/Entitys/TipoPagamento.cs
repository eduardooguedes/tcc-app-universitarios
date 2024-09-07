using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class TipoPagamento
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public bool CartaoCredito { get; set; }

    public bool Pix { get; set; }

    public string? Imagem { get; set; }

    public bool FormaComum { get; set; }

    public virtual ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
}
