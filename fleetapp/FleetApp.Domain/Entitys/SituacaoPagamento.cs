using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class SituacaoPagamento
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public string DescricaoGateway { get; set; } = null!;

    public bool Aguardando { get; set; }

    public bool EmAnalise { get; set; }

    public bool Pago { get; set; }

    public bool Cancelado { get; set; }

    public bool Rejeitado { get; set; }

    public bool Expirado { get; set; }

    public bool EstornarAoCancelarPedido { get; set; }

    public virtual ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
}
