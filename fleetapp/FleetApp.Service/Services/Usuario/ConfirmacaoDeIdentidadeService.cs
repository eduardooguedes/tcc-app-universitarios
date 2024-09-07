using Dashdine.CrossCutting.CrossCutting.Enums.Usuario;
using Dashdine.CrossCutting.Enums.Usuario;
using Dashdine.Service.Interface.Email;
using Dashdine.Service.Interface.Usuario;
using Dashdine.Service.Interface.Usuario.MemoriaCache;

namespace Dashdine.Service.Services.Usuario;

public class ConfirmacaoDeIdentidadeService(IMemoriaCacheConfirmacaoEmailService cacheConfirmacaoEmailService, IMemoriaCacheRecuperacaoDeSenhaService recuperacaoDeSenhaService, IEmailService emailService) : IConfirmacaoDeIdentidadeService
{
    public async Task<double> EnviarConfirmacaoParaAtualizarEmail(string idUsuario, string nomeUsuario, string emailNovo, EnumTipoDeUsuario tipoDeUsuario)
    {
        const int TEMPO_EM_MINUTOS_PARA_EXPIRAR_CACHE = 8;
        var codigoGerado = cacheConfirmacaoEmailService.AdicionarEObterCodigoGerado(idUsuario, emailNovo, tipoDeUsuario, TEMPO_EM_MINUTOS_PARA_EXPIRAR_CACHE);
        //await emailService.Enviar(emailNovo, "Confirmação de e-mail", $"Olá, {nomeUsuario}. <br/> Segue código para confirmação de e-mail: <br/><br/> {codigoGerado} <br/><br/>Ninguêm do Dashdine irá solicitar esse código. Não informe para ninguém!");

        return TimeSpan.FromMinutes(TEMPO_EM_MINUTOS_PARA_EXPIRAR_CACHE).TotalSeconds;
    }

    public async Task<double> EnviarConfirmacaoParaRecuperarSenha(string idUsuario, string nomeUsuario, string emailUsuario, EnumTipoDeUsuario tipoDeUsuario, EnumCanalDeContato canalDeContato)
    {
        const double MINUTOS_PARA_EXPIRAR = 8;
        string codigo = recuperacaoDeSenhaService.AdicionarEObterCodigoGerado(idUsuario, tipoDeUsuario, MINUTOS_PARA_EXPIRAR);
        //await emailService.Enviar(emailUsuario, "Recuperação de senha", $"Olá, {nomeUsuario}. <br/> Segue código para recuperação de senha: <br/><br/> {codigo} <br/><br/>Ninguém do Dashdine irá solicitar esse código. Não informe para ninguém!");
        return TimeSpan.FromMinutes(MINUTOS_PARA_EXPIRAR).TotalSeconds;
    }

    public string? ConfirmarCodigoEObterNovoEmail(Guid idUsuario, string codigo, EnumTipoDeUsuario tipoDeUsuario)
    {
        return cacheConfirmacaoEmailService.ObterNovoEmail(codigo, idUsuario.ToString(), tipoDeUsuario);
    }

    public bool ConfirmarIdentidadeEAutorizarAlteracaoSenha(Guid idUsuario, string codigo, EnumTipoDeUsuario tipoDeUsuario)
    {
        if (recuperacaoDeSenhaService.EstaAutorizadoAlterarSenha(idUsuario.ToString(), codigo, tipoDeUsuario))
        {
            recuperacaoDeSenhaService.LimparCacheUsuario(idUsuario.ToString(), tipoDeUsuario);
            return true;
        }
        return false;
    }
}
