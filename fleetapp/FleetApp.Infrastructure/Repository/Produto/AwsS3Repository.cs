using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Dashdine.Infrastructure.Repository.Produto
{
    public class AwsS3Repository(string chaveDeAcesso, string chaveSecretaDeAcesso, string nomeDoBucket, string regiao)
    {
        private readonly RegionEndpoint regiao = RegionEndpoint.GetBySystemName(regiao);

        private async Task<Stream> formatarImagemParaPng600x400(IFormFile arquivo)
        {
            MemoryStream outStream = new();
            Image imagem = Image.Load(arquivo.OpenReadStream());
            imagem
                .Mutate(i => i
                            .Resize(new ResizeOptions
                            {
                                Size = new Size(600, 400),
                                Mode = ResizeMode.Min,
                            })
                        );

            await imagem.SaveAsPngAsync(outStream);
            return outStream;
        }

        public async Task<bool> AtualizarObjeto(string chave, IFormFile arquivo)
        {
            if (string.IsNullOrEmpty(chaveDeAcesso) || string.IsNullOrEmpty(chaveSecretaDeAcesso) || string.IsNullOrEmpty(nomeDoBucket) || regiao == null)
                throw new ArgumentNullException("Configurações para conexão com AWS incompletas.");

            Stream outStream = await formatarImagemParaPng600x400(arquivo);

            AmazonS3Client client = new(chaveDeAcesso, chaveSecretaDeAcesso, regiao);
            PutObjectRequest request = new()
            {

                BucketName = nomeDoBucket,
                Key = chave,
                InputStream = outStream,
                ContentType = arquivo.ContentType,
            };

            return (await client.PutObjectAsync(request)).HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<bool> RemoverObjeto(string chave)
        {
            AmazonS3Client client = new(chaveDeAcesso, chaveSecretaDeAcesso, regiao);
            return (await client.DeleteObjectAsync(nomeDoBucket, chave))
                .HttpStatusCode == System.Net.HttpStatusCode.NoContent;
        }

        public string ObterUrlObjeto(string key)
        {
            return $"https://{nomeDoBucket}.s3.{regiao.SystemName}.amazonaws.com/{key}";
        }
    }
}
