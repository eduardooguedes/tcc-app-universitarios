using Dashdine.Domain.Interface.Produto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Dashdine.Infrastructure.Repository.Produto;

public class ImagemDoProdutoAwsRepository : IImagemDoProdutoRepository
{
    private readonly string mensagemErroAoAtualizarImagem = "Não foi possível atualizar imagem do produto.";
    private readonly string mensagemErroAoRemoverImagem = "Não foi possível remover a imagem do produto.";
    private readonly string contentType = ".png";
    private readonly IConfiguration configuration;

    public ImagemDoProdutoAwsRepository(IConfiguration configuration) => this.configuration = configuration;

    private string FormatarUrl(Guid idEstabelecimento, Guid idProduto)
    {
        return $"produtos/{idEstabelecimento}/{idProduto}{contentType}";
    }

    public async Task<string> AtualizarImagem(Guid idEstabelecimento, Guid idProduto, IFormFile imagem)
    {
        if (imagem == null || imagem.Length <= 0)
            throw new Exception("Carregue uma imagem para atualizar.");

        var s3Repository = ObterS3Repository() ?? throw new Exception(mensagemErroAoAtualizarImagem + " Contate o suporte.");

        if (!await s3Repository.AtualizarObjeto(FormatarUrl(idEstabelecimento, idProduto), imagem))
            throw new Exception(mensagemErroAoAtualizarImagem + " Tente novamente mais tarde.");

        return $"{s3Repository.ObterUrlObjeto(FormatarUrl(idEstabelecimento, idProduto))}";
    }

    public async Task RemoverImagem(Guid idEstabelecimento, Guid idProduto)
    {
        var s3Repository = ObterS3Repository() ?? throw new Exception(mensagemErroAoRemoverImagem + " Contate o suporte.");
        if (!await s3Repository.RemoverObjeto(FormatarUrl(idEstabelecimento, idProduto)))
            throw new Exception(mensagemErroAoRemoverImagem + " Tente novamente mais tarde.");
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
