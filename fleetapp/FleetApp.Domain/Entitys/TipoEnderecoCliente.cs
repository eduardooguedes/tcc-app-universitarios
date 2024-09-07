using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class TipoEnderecoCliente
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public bool Cobranca { get; set; }

    public virtual ICollection<EnderecoCliente> EnderecoClientes { get; set; } = new List<EnderecoCliente>();
}
