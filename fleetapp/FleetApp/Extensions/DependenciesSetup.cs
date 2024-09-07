using Dashdine.Domain.Interface.Cliente;
using Dashdine.Domain.Interface.Estabelecimento;
using Dashdine.Domain.Interface.Geolocalizacao;
using Dashdine.Domain.Interface.Produto;
using Dashdine.Infrastructure.Repository.Cliente;
using Dashdine.Infrastructure.Repository.Estabelecimento;
using Dashdine.Infrastructure.Repository.Pedido;
using Dashdine.Infrastructure.Repository.Produto;
using Dashdine.Service.Interface.Cliente;
using Dashdine.Service.Interface.Email;
using Dashdine.Service.Interface.Estabelecimento;
using Dashdine.Service.Interface.Produto;
using Dashdine.Service.Interface.Usuario;
using Dashdine.Service.Interface.Usuario.MemoriaCache;
using Dashdine.Service.Services.Cliente;
using Dashdine.Service.Services.Email;
using Dashdine.Service.Services.Estabelecimento;
using Dashdine.Service.Services.Produto;
using Dashdine.Service.Services.Usuario;
using Dashdine.Service.Services.Usuario.Autenticacao;
using Dashdine.Service.Services.Usuario.MemoriaCache;
using Dashdine.Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Dashdine.Domain.Interface.Pedido;
using Dashdine.Domain.Interface;
using Dashdine.Infrastructure.Repository;
using Dashdine.Infrastructure.Repository.Pagamento;
using Dashdine.Domain.Interface.Pagamento;
using Dashdine.Infrastructure.Repository.Geolocalizacao;

namespace Dashdine.Application.Extensions;

public static class DependenciesSetup
{
    /// <summary>
    /// Adiciona dependencias do sistema: serviços, repositorios, memoria cache e contexto.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        #region Dependencias

        //services.AddDbContextFactory<EntityContext>(option => option.UseSqlServer(configuration["ConnectionsStrings:ServerConnection"]));
        services.AddEntityFrameworkNpgsql()
            .AddDbContext<EntityContext>(option =>
                option.UseNpgsql(configuration["ConnectionsStrings:ServerConnection"]));

        #region Services
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IMemoriaCacheRecuperacaoDeSenhaService, MemoriaCacheRecuperacaoDeSenhaService>();
        services.AddSingleton<IMemoriaCacheConfirmacaoEmailService, MemoriaCacheConfirmacaoEmailService>();

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAutenticacaoService, AutenticacaoService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IConfirmacaoDeIdentidadeService, ConfirmacaoDeIdentidadeService>();
        services.AddScoped<IEnderecoClienteService, EnderecoClienteService>();
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<ICartoesClienteService, CartoesClienteService>();
        services.AddScoped<IGestorService, GestorService>();
        services.AddScoped<IEstabelecimentoService, EstabelecimentoService>();
        services.AddScoped<IProdutoService, ProdutoService>();
        services.AddScoped<IAdicionalDeProdutoService, AdicionalDeProdutoService>();
        services.AddScoped<IConfiguracoesProdutoService, ConfiguracoesProdutoService>();
        services.AddScoped<IEstabelecimentosClienteService, EstabelecimentosClienteService>();
        services.AddScoped<IPedidoClienteService, PedidoClienteService>();
        services.AddScoped<IPedidoEstabelecimentoService, PedidoEstabelecimentoService>();
        services.AddScoped<IPagamentoPedidoClienteService, PagamentoPedidoClienteService>();
        #endregion

        #region Repositories
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<ICartoesClienteRepository, CartoesClienteRepository>();
        services.AddScoped<IEnderecoClienteRepository, EnderecoClienteRepository>();
        services.AddScoped<IGestorRepository, GestorRepository>();
        services.AddScoped<IEstabelecimentoRepository, EstabelecimentoRepository>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IAdicionalRepository, AdicionalRepository>();
        services.AddScoped<IImagemDoProdutoRepository, ImagemDoProdutoAwsRepository>();
        services.AddScoped<ILogoDoEstabelecimentoRepository, LogoDoEstabelecimentoAwsRepository>();
        services.AddScoped<IGeolocalizacaoRepository, GeocodingGoogleRepository>();
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<ISituacaoPedidoRepository, SituacaoPedidoRepository>();
        services.AddScoped<ITimeZoneRepository, TimeZoneRepository>();
        services.AddScoped<ICategoriaDeProdutoRepository, CategoriaDeProdutoRepository>();
        services.AddScoped<ITipoDeProdutoRepository, TipoDeProdutoRepository>();
        services.AddScoped<ITipoEnderecoClienteRepository, TipoEnderecoClienteRepository>();
        services.AddScoped<ISituacaoProdutoRepository, SituacaoProdutoRepository>();
        services.AddScoped<IParametroRepository, ParametroRepository>();
        services.AddScoped<IPagamentoRepository, PagamentoRepository>();
        services.AddScoped<ISituacaoPagamentoRepository, SituacaoPagamentoRepository>();
        services.AddScoped<ITipoPagamentoRepository, TipoPagamentoRepository>();
        #endregion
        #endregion
    }
}
