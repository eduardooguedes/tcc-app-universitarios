using Dashdine.CrossCutting.Extensions;
using Dashdine.Domain.Domain.Produto;

namespace Dashdine.Domain.Domain.Pedido;

public class AdicionalDoProdutoDoPedidoDomain
{
    public Guid IdProdutoDoPedido { get; }
    public Guid IdAdicional { get; }
    public SituacaoDeProdutoDomain Situacao { get; }
    public string Nome { get; }
    public int Quantidade { get; }
    public decimal PrecoUnitario { get; }
    public decimal PrecoTotal { get; }
    public int? QuantidadeMaxima { get; }

    public AdicionalDoProdutoDoPedidoDomain(Guid idAdicional, Guid idProdutoDoPedido, SituacaoDeProdutoDomain situacao, string nome, int quantidade, decimal preco, int? quantidadeMaxima)
    {
        IdAdicional = idAdicional;
        IdProdutoDoPedido = idProdutoDoPedido;
        Situacao = situacao;
        Nome = nome;
        Quantidade = quantidade;
        PrecoUnitario = preco;
        PrecoTotal = (quantidade * preco).ArredondarPreco();
        QuantidadeMaxima = quantidadeMaxima;
    }
}
