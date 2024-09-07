using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Service.Models.Usuario;

namespace Dashdine.Service.Interface.Usuario;

public interface IUsuarioService
{
    Task<double> SolicitarParaAtualizarEmail(UsuarioAutenticado usuario, string novoEmail);
    Task<bool> ConfirmarAtualizacaoDeEmail(UsuarioAutenticado usuarioAutenticado, string codigo);
    Task<ProjecaoDeUsuarioLogado> RecuperarSenha(UsuarioAutenticado usuarioAutenticado, DtoDeRecuperacaoDeSenha dto);
    Task AlterarSenha(UsuarioAutenticado usuario, DtoDeAlteracaoDeSenha dto);
}
