using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class EnderecoCliente
{
    public Guid Id { get; set; }

    public Guid IdCliente { get; set; }

    public int IdTipoEndereco { get; set; }

    public string Apelido { get; set; } = null!;

    public bool Principal { get; set; }

    public string Cep { get; set; } = null!;

    public string Logradouro { get; set; } = null!;

    public int Numero { get; set; }

    public string? Complemento { get; set; }

    public string? Bairro { get; set; }

    public string Cidade { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public string Timezone { get; set; } = null!;

    public virtual ICollection<CartaoCliente> CartaoClientes { get; set; } = new List<CartaoCliente>();

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual TipoEnderecoCliente IdTipoEnderecoNavigation { get; set; } = null!;

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
