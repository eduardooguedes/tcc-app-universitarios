using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class ProdutoPedido
{
    public Guid Id { get; set; }

    public Guid IdPedido { get; set; }

    public Guid IdProduto { get; set; }

    public int Quantidade { get; set; }

    public decimal PrecoProduto { get; set; }

    public decimal PrecoUnitario { get; set; }

    public decimal PrecoTotal { get; set; }

    public int? Nota { get; set; }

    public virtual ICollection<AdicionalProdutoPedido> AdicionalProdutoPedidos { get; set; } = new List<AdicionalProdutoPedido>();

    public virtual Pedido IdPedidoNavigation { get; set; } = null!;

    public virtual Produto IdProdutoNavigation { get; set; } = null!;
}
