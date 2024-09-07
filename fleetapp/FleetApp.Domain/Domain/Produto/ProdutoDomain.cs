namespace Dashdine.Domain.Domain.Produto
{
    public class ProdutoDomain : EntidadeBase
    {
        public CategoriaDeProduto Categoria { get; set; }
        public SituacaoDeProdutoDomain Situacao { get; set; }
        public TipoDoProdutoDomain Tipo { get; set; }
        public Guid IdEstabelecimento { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public string? Imagem { get; set; }
        public decimal Preco { get; set; }
        public int TempoEmMinutosParaRetirada { get; set; }
        public int QuantidadeVezesVendido { get; set; }
        public decimal? NotaMedia { get; set; }
        public int QuantidadeDeVotos { get; set; }
        public List<AdicionalDoProdutoDomain>? Adicionais { get; set; }

        public ProdutoDomain(Guid idEstabelecimento, SituacaoDeProdutoDomain situacao, CategoriaDeProduto categoria, TipoDoProdutoDomain tipo, string nome, string? descricao, decimal preco,
            int tempoEmMinutosParaRetirada, string? imagem, IEnumerable<AdicionalDoProdutoDomain>? adicionais)
        {
            IdEstabelecimento = idEstabelecimento;
            Situacao = situacao;
            Categoria = categoria;
            Tipo = tipo;
            Nome = nome;
            Descricao = descricao;
            Preco = preco;
            Imagem = imagem;
            TempoEmMinutosParaRetirada = tempoEmMinutosParaRetirada;
            QuantidadeVezesVendido = 0;
            NotaMedia = 0;
            QuantidadeDeVotos = 0;
            Adicionais = adicionais?.ToList();
        }

        public ProdutoDomain(Guid id, Guid idEstabelecimento, SituacaoDeProdutoDomain situacao, CategoriaDeProduto categoria, TipoDoProdutoDomain tipo, string nome, string? descricao, decimal preco,
            int tempoEmMinutosParaRetirada, string? imagem, int quantidadeVezesVendido, decimal? notaMedia, int quantidadeDeVotos, IEnumerable<AdicionalDoProdutoDomain>? adicionais)
            : this(idEstabelecimento, situacao, categoria, tipo, nome, descricao, preco, tempoEmMinutosParaRetirada, imagem, adicionais)
        {
            Id = id;
            QuantidadeVezesVendido = quantidadeVezesVendido;
            NotaMedia = notaMedia;
            QuantidadeDeVotos = quantidadeDeVotos;
        }

        public void AtualizarSituacao(SituacaoDeProdutoDomain situacaoDeProduto)
        {
            this.Situacao = situacaoDeProduto;
        }
    }
}
