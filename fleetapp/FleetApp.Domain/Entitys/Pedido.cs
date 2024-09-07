using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class Pedido
{
    public Guid Id { get; set; }

    public string? IdGateway { get; set; }

    public Guid IdCliente { get; set; }

    public Guid IdLocalizacaoCliente { get; set; }

    public Guid IdEstabelecimento { get; set; }

    public int IdSituacao { get; set; }

    public int? IdDestino { get; set; }

    public int? Numero { get; set; }

    public DateTime DataHora { get; set; }

    public DateTime? DataHoraARetirar { get; set; }

    public string? Observacao { get; set; }

    public string? ObservacaoEstabelecimento { get; set; }

    public decimal ValorTotal { get; set; }

    public DateTime? DataHoraRetirado { get; set; }

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual DestinoPedido? IdDestinoNavigation { get; set; }

    public virtual Estabelecimento IdEstabelecimentoNavigation { get; set; } = null!;

    public virtual EnderecoCliente IdLocalizacaoClienteNavigation { get; set; } = null!;

    public virtual SituacaoPedido IdSituacaoNavigation { get; set; } = null!;

    public virtual ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();

    public virtual ICollection<ProdutoPedido> ProdutoPedidos { get; set; } = new List<ProdutoPedido>();
}
