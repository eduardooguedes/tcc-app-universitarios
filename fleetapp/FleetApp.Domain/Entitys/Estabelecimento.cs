using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class Estabelecimento
{
    public Guid Id { get; set; }

    public int IdSituacao { get; set; }

    public DateTime DataHoraCadastro { get; set; }

    public string NomeFantasia { get; set; } = null!;

    public string RazaoSocial { get; set; } = null!;

    public string Cnpj { get; set; } = null!;

    public string? Telefone { get; set; }

    public string? Logo { get; set; }

    public virtual ICollection<Adicional> Adicionals { get; set; } = new List<Adicional>();

    public virtual ICollection<EnderecoEstabelecimento> EnderecoEstabelecimentos { get; set; } = new List<EnderecoEstabelecimento>();

    public virtual ICollection<Gestor> Gestors { get; set; } = new List<Gestor>();

    public virtual ICollection<HorariosFuncionamento> HorariosFuncionamentos { get; set; } = new List<HorariosFuncionamento>();

    public virtual SituacaoEstabelecimento IdSituacaoNavigation { get; set; } = null!;

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
