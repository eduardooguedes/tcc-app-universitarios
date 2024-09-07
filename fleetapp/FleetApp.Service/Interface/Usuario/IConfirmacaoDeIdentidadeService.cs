using Dashdine.CrossCutting.CrossCutting.Enums.Usuario;
using Dashdine.CrossCutting.Enums.Usuario;

namespace Dashdine.Service.Interface.Usuario;

public interface IConfirmacaoDeIdentidadeService
{
    Task<double> EnviarConfirmacaoParaAtualizarEmail(string idUsuario, string nomeUsuario, string emailNovo, EnumTipoDeUsuario tipoDeUsuario);
    Task<double> EnviarConfirmacaoParaRecuperarSenha(string idUsuario, string nomeUsuario, string emailUsuario, EnumTipoDeUsuario tipoDeUsuario, EnumCanalDeContato canalDeContato);
    string? ConfirmarCodigoEObterNovoEmail(Guid idUsuario, string codigo, EnumTipoDeUsuario tipoDeUsuario);
    bool ConfirmarIdentidadeEAutorizarAlteracaoSenha(Guid idUsuario, string codigo, EnumTipoDeUsuario tipoDeUsuario);
}
