using Dashdine.CrossCutting.Enums.Usuario;

namespace Dashdine.Service.Interface.Usuario;

public interface ITokenService
{
    string GerarToken(Guid id, string nome, string email, string celular, EnumTipoDeUsuario tipoDeUsuario, int situacao, bool autorizadoRecuperarSenha = false);
}
