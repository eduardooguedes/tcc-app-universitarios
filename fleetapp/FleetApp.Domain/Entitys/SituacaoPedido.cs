using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class SituacaoPedido
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public string CorHexadecimal { get; set; } = null!;

    public bool EmAndamento { get; set; }

    public bool Cancelado { get; set; }

    public bool Aceito { get; set; }

    public bool Novo { get; set; }

    public bool Retirado { get; set; }

    public bool AguardandoPagamento { get; set; }

    public bool Rejeitado { get; set; }

    public bool ListadoParaCliente { get; set; }

    public bool ListadoParaGestor { get; set; }

    public bool PossuiCategoriaPropriaParaCliente { get; set; }

    public bool PermitidoCancelar { get; set; }

    public int? IntervaloMinimoEmHorasAntesDeRetirarParaCancelar { get; set; }

    public bool ApagarPedidoAoCancelar { get; set; }

    public bool PermitidoSolicitarAjuda { get; set; }

    public bool PermitidoRetirar { get; set; }

    public bool PermitidoPedirNovamente { get; set; }

    public bool PermitidoRejeitar { get; set; }

    public bool VisualizaMotivo { get; set; }

    public bool DiminuiQuantidadeDePedidosDisponiveis { get; set; }

    public bool PermitidoPagar { get; set; }

    public bool EstornarPagamentoAoCancelar { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
