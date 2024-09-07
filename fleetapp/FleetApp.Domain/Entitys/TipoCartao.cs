using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class TipoCartao
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<CartaoCliente> CartaoClientes { get; set; } = new List<CartaoCliente>();
}
