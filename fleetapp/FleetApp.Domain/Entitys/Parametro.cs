using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class Parametro
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public string Valor { get; set; } = null!;
}
