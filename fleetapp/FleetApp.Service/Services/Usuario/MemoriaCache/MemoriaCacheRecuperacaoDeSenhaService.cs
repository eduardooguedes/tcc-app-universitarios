using Dashdine.CrossCutting.CrossCutting.CodigoDeAutenticidade;
using Dashdine.CrossCutting.Enums.Usuario;
using Dashdine.Domain.Domain.Usuario.Cache;
using Dashdine.Service.Interface.Usuario.MemoriaCache;
using Microsoft.Extensions.Caching.Memory;

namespace Dashdine.Service.Services.Usuario.MemoriaCache;

public class MemoriaCacheRecuperacaoDeSenhaService : IMemoriaCacheRecuperacaoDeSenhaService
{
    private readonly IMemoryCache memoriaCache;

    public MemoriaCacheRecuperacaoDeSenhaService(IMemoryCache memoriaCache)
    {
        this.memoriaCache = memoriaCache;
    }

    public string AdicionarEObterCodigoGerado(string idUsuario, EnumTipoDeUsuario tipoUsuario, double minutosParaExpirar)
    {
        var keyCache = ObterKeyCache(idUsuario, tipoUsuario);
        string codigo = ObterCodigoAtual(keyCache);
        if (!string.IsNullOrEmpty(codigo))
            return codigo;

        codigo = CodigoAutenticidade.GerarCodigoAleatorioSeisDigitos();
        CacheRecuperacaoSenha cacheRecuperacaoSenha = new()
        {
            CodigoGerado = codigo,
            TipoDeUsuario = tipoUsuario,
            IdUsuario = idUsuario,
        };

        var cacheEntryOptions = new MemoryCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(minutosParaExpirar), AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(minutosParaExpirar + 2) };
        memoriaCache.Set(keyCache, cacheRecuperacaoSenha, cacheEntryOptions);
        return codigo;
    }

    private string ObterCodigoAtual(string keyCache)
    {
        if (memoriaCache.TryGetValue(keyCache, out CacheRecuperacaoSenha? memoria) && memoria is not null)
            return memoria.CodigoGerado;

        return string.Empty;
    }

    public bool EstaAutorizadoAlterarSenha(string idUsuario, string codigo, EnumTipoDeUsuario tipoDeUsuario)
    {
        if (memoriaCache.TryGetValue(ObterKeyCache(idUsuario, tipoDeUsuario), out CacheRecuperacaoSenha? recuperacao) && recuperacao is not null)
            return recuperacao.CodigoGerado == codigo;

        return false;
    }

    public void LimparCacheUsuario(string idUsuario, EnumTipoDeUsuario tipoDeUsuario)
    {
        memoriaCache.Remove(ObterKeyCache(idUsuario, tipoDeUsuario));
    }

    private static string ObterKeyCache(string idUsuario, EnumTipoDeUsuario tipoDeUsuario)
    {
        return idUsuario + "|" + tipoDeUsuario.ToString();
    }
}
