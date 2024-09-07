using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class HorariosFuncionamento
{
    public Guid Id { get; set; }

    public Guid IdEstabelecimento { get; set; }

    public int IdSituacao { get; set; }

    public string Dias { get; set; } = null!;

    public TimeOnly InicioHorario { get; set; }

    public TimeOnly FimHorario { get; set; }

    public string DestinosDaRetirada { get; set; } = null!;

    public int IntervaloEmMinutosEntreRetiradas { get; set; }

    public int? QtdePedidosPorRetirada { get; set; }

    public int? QtdePedidosPrimeiraRetirada { get; set; }

    public int? QtdeProdutosPorPedido { get; set; }

    public int? MinutosEntrePedirERetirar { get; set; }

    public virtual Estabelecimento IdEstabelecimentoNavigation { get; set; } = null!;

    public virtual SituacaoHorariosFuncionamento IdSituacaoNavigation { get; set; } = null!;
}
