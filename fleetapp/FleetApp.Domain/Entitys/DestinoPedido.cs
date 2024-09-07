using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class DestinoPedido
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
