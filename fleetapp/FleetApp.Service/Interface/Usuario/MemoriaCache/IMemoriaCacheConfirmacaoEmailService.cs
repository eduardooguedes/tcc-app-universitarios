using Dashdine.CrossCutting.Enums.Usuario;

namespace Dashdine.Service.Interface.Usuario.MemoriaCache;

public interface IMemoriaCacheConfirmacaoEmailService
{
    string AdicionarEObterCodigoGerado(string idUsuario, string emailNovo, EnumTipoDeUsuario tipoDeUsuario, int tempoEmMinutosParaExpirar);
    string? ObterNovoEmail(string codigo, string idUsuario, EnumTipoDeUsuario tipoDeUsuario);
}
