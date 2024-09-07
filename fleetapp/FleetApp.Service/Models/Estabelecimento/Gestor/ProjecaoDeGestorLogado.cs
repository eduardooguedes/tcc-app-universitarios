namespace Dashdine.Service.Models.Estabelecimento.Gestor;

public class ProjecaoDeGestorLogado
{
    public string Token { get; set; }
    public ProjecaoDeGestor Gestor { get; set; }
    public List<ProjecaoParaListaDeEstabelecimentosDoGestor> Estabelecimentos { get; set; }
}
