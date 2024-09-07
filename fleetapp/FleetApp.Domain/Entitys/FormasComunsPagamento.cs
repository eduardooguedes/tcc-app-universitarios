using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class FormasComunsPagamento
{
    public Guid Id { get; set; }

    public int IdTipoPagamento { get; set; }

    public string? Imagem { get; set; }

    public string Nome { get; set; } = null!;

    public string? Descricao { get; set; }

    public virtual TipoPagamento IdTipoPagamentoNavigation { get; set; } = null!;
}
