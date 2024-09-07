using Dashdine.CrossCutting.CrossCutting.CodigoDeAutenticidade;
using Dashdine.CrossCutting.Enums.Usuario;
using Dashdine.Domain.Domain.Usuario.Cache;
using Dashdine.Service.Interface.Usuario.MemoriaCache;
using Microsoft.Extensions.Caching.Memory;

namespace Dashdine.Service.Services.Usuario.MemoriaCache;

public class MemoriaCacheConfirmacaoEmailService : IMemoriaCacheConfirmacaoEmailService
{
    private readonly IMemoryCache memoriaCache;

    public MemoriaCacheConfirmacaoEmailService(IMemoryCache memoriaCache)
    {
        this.memoriaCache = memoriaCache;
    }

    public string AdicionarEObterCodigoGerado(string idUsuario, string emailNovo, EnumTipoDeUsuario tipoDeUsuario, int tempoEmMinutosParaExpirar)
    {
        var keyCache = ObterKeyCache(idUsuario, tipoDeUsuario);
        string codigo = ObterCodigoAtual(keyCache);
        if (!string.IsNullOrEmpty(codigo))
            return codigo;

        codigo = CodigoAutenticidade.GerarCodigoAleatorioSeisDigitos();
        CacheAlteracaoEmail cacheRecuperacaoSenha = new()
        {
            CodigoGerado = codigo,
            IdUsuario = idUsuario,
            NovoEmail = emailNovo,
            TipoDeUsuario = tipoDeUsuario
        };

        var cacheEntryOptions = new MemoryCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(tempoEmMinutosParaExpirar), AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(tempoEmMinutosParaExpirar + 2) };
        memoriaCache.Set(keyCache, cacheRecuperacaoSenha, cacheEntryOptions);
        return codigo;
    }

    private string ObterCodigoAtual(string keyCache)
    {
        if (memoriaCache.TryGetValue(keyCache, out CacheAlteracaoEmail? memoria) && memoria is not null)
            return memoria.CodigoGerado;

        return string.Empty;
    }

    public string? ObterNovoEmail(string codigo, string idUsuario, EnumTipoDeUsuario tipoDeUsuario)
    {
        if (!memoriaCache.TryGetValue(ObterKeyCache(idUsuario, tipoDeUsuario), out CacheAlteracaoEmail? alteracao))
            return null;

        if (alteracao is null)
            return null;

        if (alteracao.CodigoGerado != codigo)
            return null;

        return alteracao.NovoEmail;
    }

    private static string ObterKeyCache(string idUsuario, EnumTipoDeUsuario tipoDeUsuario)
    {
        return idUsuario + "|" + tipoDeUsuario.ToString();
    }
}
