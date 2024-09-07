using Dashdine.CrossCutting.Enums.Usuario;
using System.Security.Claims;

namespace Dashdine.Domain.Domain.Usuario.Autenticacao;

public class UsuarioAutenticado
{
    private readonly string mensagemDeErro = "Usuario não autenticado.";
    private bool autorizadoRecuperarSenha = false;
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Celular { get; set; }
    public EnumTipoDeUsuario TipoDeUsuario { get; set; }
    public int SituacaoDeUsuario { get; set; }

    public UsuarioAutenticado(ClaimsPrincipal user)
    {
        string? id = user.Claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(id))
            throw new Exception(mensagemDeErro);

        Id = Guid.Parse(id);
        Nome = user.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Name)?.Value ?? throw new Exception(mensagemDeErro);
        Email = user.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Email)?.Value ?? throw new Exception(mensagemDeErro);
        Celular = user.Claims.FirstOrDefault(i => i.Type == ClaimTypes.MobilePhone)?.Value ?? throw new Exception(mensagemDeErro);

        if (!Enum.TryParse(user.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Role)?.Value, out EnumTipoDeUsuario tipo))
            throw new Exception(mensagemDeErro);
        TipoDeUsuario = tipo;

        if (!int.TryParse(user.Claims.FirstOrDefault(i => i.Type == "Situacao")?.Value, out int situacao))
            throw new Exception(mensagemDeErro);
        SituacaoDeUsuario = situacao;

        if (bool.TryParse(user.Claims.FirstOrDefault(i => i.Type == "RecuperarSenha")?.Value, out bool autorizadoRecuperarSenha))
            this.autorizadoRecuperarSenha = autorizadoRecuperarSenha;
    }

    public bool AutorizadoARecuperarSenha()
    {
        if (autorizadoRecuperarSenha)
        {
            autorizadoRecuperarSenha = false;
            return true;
        }

        return false;
    }
}
