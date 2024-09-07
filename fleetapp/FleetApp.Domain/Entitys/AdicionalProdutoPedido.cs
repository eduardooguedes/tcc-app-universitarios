using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class AdicionalProdutoPedido
{
    public Guid IdProdutoPedido { get; set; }

    public Guid IdAdicionalProduto { get; set; }

    public int Quantidade { get; set; }

    public decimal PrecoUnitario { get; set; }

    public decimal PrecoTotal { get; set; }

    public virtual Adicional IdAdicionalProdutoNavigation { get; set; } = null!;

    public virtual ProdutoPedido IdProdutoPedidoNavigation { get; set; } = null!;
}
