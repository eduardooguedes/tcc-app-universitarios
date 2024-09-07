namespace Dashdine.Domain.Domain.Produto
{
    public class Adicional : EntidadeBase
    {
        public Guid IdEstabelecimento { get; }
        public SituacaoDeProdutoDomain Situacao { get; set; }
        public string? Nome { get; }
        public decimal? Preco { get; }
        public int? QuantidadeDeProdutosVinculados { get; }

        public Adicional(Guid id, Guid idEstabelecimento)
        {
            Id = id;
            IdEstabelecimento = idEstabelecimento;
        }

        public Adicional(Guid idEstabelecimento, SituacaoDeProdutoDomain situacao, string nome, decimal preco)
        {
            IdEstabelecimento = idEstabelecimento;
            Situacao = situacao;
            Nome = nome;
            Preco = preco;
        }

        public Adicional(Guid id, Guid idEstabelecimento, SituacaoDeProdutoDomain situacao, string nome, decimal preco, int? quantidadeDeProdutosVinculados = null) : this(idEstabelecimento, situacao, nome, preco)
        {
            Id = id;
            QuantidadeDeProdutosVinculados = quantidadeDeProdutosVinculados;
        }

        public void AtualizarSituacao(SituacaoDeProdutoDomain situacaoDeProduto)
        {
            Situacao = situacaoDeProduto;
        }
    }
}
