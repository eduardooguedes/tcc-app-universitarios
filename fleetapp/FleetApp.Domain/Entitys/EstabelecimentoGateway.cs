using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class EstabelecimentoGateway
{
    public Guid Id { get; set; }

    public Guid IdEstabelecimento { get; set; }

    public string IdCarteiraGateway { get; set; } = null!;
}
