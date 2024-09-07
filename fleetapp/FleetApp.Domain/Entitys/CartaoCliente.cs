using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class CartaoCliente
{
    public Guid Id { get; set; }

    public Guid IdCliente { get; set; }

    public Guid IdEndereco { get; set; }

    public int IdTipo { get; set; }

    public string? Apelido { get; set; }

    public string FinalDoNumero { get; set; } = null!;

    public string Bandeira { get; set; } = null!;

    public DateOnly Validade { get; set; }

    public virtual ICollection<CartaoClienteGateway> CartaoClienteGateways { get; set; } = new List<CartaoClienteGateway>();

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual EnderecoCliente IdEnderecoNavigation { get; set; } = null!;

    public virtual TipoCartao IdTipoNavigation { get; set; } = null!;
}
