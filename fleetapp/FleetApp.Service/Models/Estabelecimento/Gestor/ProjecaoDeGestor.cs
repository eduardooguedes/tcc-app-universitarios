namespace Dashdine.Service.Models.Estabelecimento.Gestor;

public class ProjecaoDeGestor
{
    public string Id { get; set; }
    public int IdNivelGestao { get; set; }
    public int IdSituacao { get; set; }
    public string CPF { get; set; }
    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmado { get; set; }
    public DateTime DataDeCadastro { get; set; }
    public DateOnly DataDeNascimento { get; set; }
}
