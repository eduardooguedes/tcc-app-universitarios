namespace Dashdine.Service.Models.Estabelecimento;

public class ProjecaoDeProdutoParaEstabelecimento
{
    public string IdCategoria { get; set; }
    public string Categoria { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public TimeSpan TempoParaRetirada { get; set; }
    public decimal Valor { get; set; }
    public int IdSituacao { get; set; }
    public string Situacao { get; set; }
}
