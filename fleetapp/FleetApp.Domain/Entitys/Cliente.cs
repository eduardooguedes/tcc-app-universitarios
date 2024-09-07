using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class Cliente
{
    public Guid Id { get; set; }

    public int IdSituacao { get; set; }

    public DateTime DataHoraCadastro { get; set; }

    public DateOnly DataNascimento { get; set; }

    public string Nome { get; set; } = null!;

    public string Sobrenome { get; set; } = null!;

    public string Cpf { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool EmailConfirmado { get; set; }

    public string Celular { get; set; } = null!;

    public bool CelularConfirmado { get; set; }

    public string Senha { get; set; } = null!;

    public virtual ICollection<CartaoCliente> CartaoClientes { get; set; } = new List<CartaoCliente>();

    public virtual ICollection<ClienteGateway> ClienteGateways { get; set; } = new List<ClienteGateway>();

    public virtual ICollection<EnderecoCliente> EnderecoClientes { get; set; } = new List<EnderecoCliente>();

    public virtual SituacaoCliente IdSituacaoNavigation { get; set; } = null!;

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
