using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class CartaoClienteGateway
{
    public Guid Id { get; set; }

    public Guid IdCartaoCliente { get; set; }

    public string IdCartaoGateway { get; set; } = null!;

    public virtual CartaoCliente IdCartaoClienteNavigation { get; set; } = null!;
}
