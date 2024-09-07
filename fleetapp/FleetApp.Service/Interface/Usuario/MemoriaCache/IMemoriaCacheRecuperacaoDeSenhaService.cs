using Dashdine.CrossCutting.Enums.Usuario;

namespace Dashdine.Service.Interface.Usuario.MemoriaCache;

public interface IMemoriaCacheRecuperacaoDeSenhaService
{
    string AdicionarEObterCodigoGerado(string idUsuario, EnumTipoDeUsuario tipoUsuario, double minutosParaExpirar);
    bool EstaAutorizadoAlterarSenha(string idUsuario, string codigo, EnumTipoDeUsuario tipoDeUsuario);
    void LimparCacheUsuario(string v, EnumTipoDeUsuario tipoDeUsuario);
}
