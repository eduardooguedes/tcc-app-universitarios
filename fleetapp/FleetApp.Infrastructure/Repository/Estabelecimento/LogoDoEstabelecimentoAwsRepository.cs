using Dashdine.Domain.Interface.Estabelecimento;
using Dashdine.Infrastructure.Repository.Produto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Dashdine.Infrastructure.Repository.Estabelecimento;

public class LogoDoEstabelecimentoAwsRepository : ILogoDoEstabelecimentoRepository
{
    private readonly IConfiguration configuration;
    private readonly string mensagemErroAoAtualizarLogo = "Não foi possível atualizar a logo do estabelecimento.";
    private readonly string mensagemErroAoRemoverLogo = "Não foi possível remover a logo do estabelecimento.";
    private readonly string contentType = ".png";

    public LogoDoEstabelecimentoAwsRepository(IConfiguration configuration) => this.configuration = configuration;

    private string FormatarUrl(Guid idEstabelecimento)
    {
        return $"logos/{idEstabelecimento}{contentType}";
    }

    public async Task<string> AtualizarLogo(Guid idEstabelecimento, IFormFile imagem)
    {
        if (imagem == null || imagem.Length <= 0)
            throw new Exception("Carregue uma imagem para atualizar a logo.");

        var s3Repository = ObterS3Repository() ?? throw new Exception(mensagemErroAoAtualizarLogo + " Contate o suporte.");

        if (!await s3Repository.AtualizarObjeto(FormatarUrl(idEstabelecimento), imagem))
            throw new Exception(mensagemErroAoAtualizarLogo + " Tente novamente mais tarde.");

        return $"{s3Repository.ObterUrlObjeto(FormatarUrl(idEstabelecimento))}";
    }

    public async Task RemoverLogo(Guid idEstabelecimento)
    {
        var s3Repository = ObterS3Repository() ?? throw new Exception(mensagemErroAoRemoverLogo + " Contate o suporte.");
        if (!await s3Repository.RemoverObjeto(FormatarUrl(idEstabelecimento)))
            throw new Exception(mensagemErroAoRemoverLogo + " Tente novamente mais tarde.");
    }

    private AwsS3Repository? ObterS3Repository()
    {
        var chaveDeAcesso = configuration["AwsS3:ChaveDeAcesso"];
        if (string.IsNullOrEmpty(chaveDeAcesso))
            return null;

        var chaveSecreta = configuration["AwsS3:ChaveSecretaDeAcesso"];
        if (string.IsNullOrEmpty(chaveSecreta))
            return null;

        var nomeDoBucket = configuration["AwsS3:NomeDoBucket"];
        if (string.IsNullOrEmpty(nomeDoBucket))
            return null;

        var regiao = configuration["AwsS3:Regiao"];
        if (string.IsNullOrEmpty(regiao))
            return null;

        return new AwsS3Repository(chaveDeAcesso, chaveSecreta, nomeDoBucket, regiao);
    }
}
