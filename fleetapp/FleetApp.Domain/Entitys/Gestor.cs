using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class Gestor
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

    public string Senha { get; set; } = null!;

    public Guid? IdEstabelecimento { get; set; }

    public virtual Estabelecimento? IdEstabelecimentoNavigation { get; set; }

    public virtual SituacaoGestor IdSituacaoNavigation { get; set; } = null!;
}
