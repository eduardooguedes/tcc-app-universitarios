using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class Pagamento
{
    public Guid Id { get; set; }

    public Guid IdPedido { get; set; }

    public string IdPedidoGateway { get; set; } = null!;

    public string? IdCobrancaGateway { get; set; }

    public int IdSituacao { get; set; }

    public int IdTipo { get; set; }

    public decimal Valor { get; set; }

    public DateTime DataHora { get; set; }

    public DateTime? DataHoraAtualizado { get; set; }

    public DateTime? DataHoraExpiracao { get; set; }

    public Guid? IdCartao { get; set; }

    public string? IdQrCodeGateway { get; set; }

    public string? TextoPix { get; set; }

    public string? ImagemQrCode { get; set; }

    public virtual Pedido IdPedidoNavigation { get; set; } = null!;

    public virtual SituacaoPagamento IdSituacaoNavigation { get; set; } = null!;

    public virtual TipoPagamento IdTipoNavigation { get; set; } = null!;
}
