using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class Produto
{
    public Guid Id { get; set; }

    public int IdCategoria { get; set; }

    public int IdSituacao { get; set; }

    public int IdTipo { get; set; }

    public Guid IdEstabelecimento { get; set; }

    public string Nome { get; set; } = null!;

    public string? Descricao { get; set; }

    public decimal Preco { get; set; }

    public string? Imagem { get; set; }

    public int MinutosParaRetirada { get; set; }

    public int QtdeVezesVendido { get; set; }

    public decimal? NotaMedia { get; set; }

    public int QtdeVotos { get; set; }

    public virtual ICollection<AdicionalProduto> AdicionalProdutos { get; set; } = new List<AdicionalProduto>();

    public virtual Categorium IdCategoriaNavigation { get; set; } = null!;

    public virtual Estabelecimento IdEstabelecimentoNavigation { get; set; } = null!;

    public virtual SituacaoProduto IdSituacaoNavigation { get; set; } = null!;

    public virtual TipoProduto IdTipoNavigation { get; set; } = null!;

    public virtual ICollection<ProdutoPedido> ProdutoPedidos { get; set; } = new List<ProdutoPedido>();
}
