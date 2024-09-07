using Dashdine.Domain.Domain.Produto;

namespace Dashdine.Service.Models.Produto;

public class DtoConfiguracoesDeProduto
{
    /// <summary>
    /// Categorias disponíveis para vincular produto.
    /// </summary>
    public IEnumerable<CategoriaDeProduto> CategoriasDeProduto { get; set; }

    /// <summary>
    /// Tipos disponíveis para definir produto.
    /// </summary>
    public IEnumerable<TipoDoProdutoDomain> TiposDeProduto { get; set; }
}
