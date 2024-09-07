using Dashdine.CrossCutting.Enums.Usuario;

namespace Dashdine.Service.Models.Usuario;

public class ProjecaoDeUsuario
{
    public string Id { get; }
    public int TipoUsuario { get; }
    public int Situacao { get; }
    public string Nome { get; }
    public string Sobrenome { get; }
    public string Email { get; }
    public bool EmailConfirmado { get; }
    public string CPF { get; }
    public string Celular { get; }
    public bool CelularConfirmado { get; }
    public DateTime DataDeCadastro { get; }
    public DateOnly DataDeNascimento { get; }

    public ProjecaoDeUsuario(Domain.Domain.Cliente.ClienteDomain usuario)
    {
        Id = usuario.Id.ToString();
        TipoUsuario = (int)EnumTipoDeUsuario.Cliente;
        Situacao = usuario.Situacao.Id;
        Nome = usuario.Nome;
        Sobrenome = usuario.Sobrenome;
        Email = usuario.Email;
        EmailConfirmado = usuario.EmailConfirmado;
        CPF = usuario.CPF;
        Celular = usuario.Celular;
        CelularConfirmado = usuario.CelularConfirmado;
        DataDeCadastro = usuario.DataHoraCadastro;
        DataDeNascimento = usuario.DataDeNascimento;
    }

    public ProjecaoDeUsuario(Domain.Domain.Estabelecimento.Gestor.Gestor usuario)
    {
        Id = usuario.Id.ToString();
        TipoUsuario = (int)EnumTipoDeUsuario.Gestor;
        Situacao = usuario.Situacao.Id;
        Nome = usuario.Nome;
        Sobrenome = usuario.Sobrenome;
        Email = usuario.Email;
        EmailConfirmado = usuario.EmailConfirmado;
        CPF = usuario.CPF;
        DataDeCadastro = usuario.DataHoraCadastro;
        DataDeNascimento = usuario.DataDeNascimento;
    }
}
