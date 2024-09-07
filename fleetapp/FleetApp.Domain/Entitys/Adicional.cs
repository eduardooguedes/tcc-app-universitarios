using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class Adicional
{
    public Guid Id { get; set; }

    public Guid IdEstabelecimento { get; set; }

    public int IdSituacao { get; set; }

    public string Nome { get; set; } = null!;

    public decimal Preco { get; set; }

    public virtual ICollection<AdicionalProdutoPedido> AdicionalProdutoPedidos { get; set; } = new List<AdicionalProdutoPedido>();

    public virtual ICollection<AdicionalProduto> AdicionalProdutos { get; set; } = new List<AdicionalProduto>();

    public virtual Estabelecimento IdEstabelecimentoNavigation { get; set; } = null!;

    public virtual SituacaoProduto IdSituacaoNavigation { get; set; } = null!;
}
