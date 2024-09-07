using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class ClienteGateway
{
    public Guid Id { get; set; }

    public Guid? IdCliente { get; set; }

    public string? IdGateway { get; set; }

    public virtual Cliente? IdClienteNavigation { get; set; }
}
