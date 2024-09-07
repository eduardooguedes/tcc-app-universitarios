using Dashdine.Domain.Domain.Produto;
using Dashdine.Domain.Entitys;
using Dashdine.Domain.Interface.Produto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dashdine.Infrastructure.Repository.Produto
{
    public class ProdutoRepository(IConfiguration configuration) : BaseRepository<Domain.Entitys.Produto>(configuration), IProdutoRepository
    {
        public async Task Cadastrar(ProdutoDomain produto)
        {
            Domain.Entitys.Produto entidade = new()
            {
                Id = produto.Id,
                Descricao = produto.Descricao,
                IdCategoria = produto.Categoria.Id,
                IdEstabelecimento = produto.IdEstabelecimento,
                IdSituacao = produto.Situacao.Id,
                IdTipo = produto.Tipo.Id,
                Nome = produto.Nome,
                NotaMedia = produto.NotaMedia,
                Preco = produto.Preco,
                QtdeVezesVendido = produto.QuantidadeVezesVendido,
                QtdeVotos = produto.QuantidadeDeVotos,
                MinutosParaRetirada = produto.TempoEmMinutosParaRetirada,
                Imagem = produto.Imagem
            };

            if (produto.Adicionais?.Count > 0)
            {
                foreach (AdicionalDoProdutoDomain adicional in produto.Adicionais)
                {
                    entidade.AdicionalProdutos.Add(new()
                    {
                        IdAdicional = adicional.Id,
                        IdProduto = entidade.Id,
                        QtdeMaxima = adicional.QuantidadeMaxima,
                    });
                }
            }

            await UnitOfWork.Produtos.AddAsync(entidade);
            await SaveChangesAsync();
        }

        public async Task Atualizar(ProdutoDomain produto)
        {
            var entidadeProduto = await UnitOfWork.Produtos
                .Include(a => a.AdicionalProdutos)
                .FirstOrDefaultAsync(p => p.Id == produto.Id);

            if (entidadeProduto is null) return;

            entidadeProduto.Nome = produto.Nome;
            entidadeProduto.Descricao = produto.Descricao;
            entidadeProduto.Imagem = produto.Imagem;
            entidadeProduto.Preco = produto.Preco;
            entidadeProduto.IdCategoria = produto.Categoria.Id;
            entidadeProduto.IdTipo = produto.Tipo.Id;
            entidadeProduto.IdSituacao = produto.Situacao.Id;
            entidadeProduto.MinutosParaRetirada = produto.TempoEmMinutosParaRetirada;

            if (produto.Adicionais?.Count > 0)
            {
                entidadeProduto.AdicionalProdutos = produto.Adicionais
                    .ToList()
                    .ConvertAll(a => new AdicionalProduto()
                    {
                        IdAdicional = a.Id,
                        QtdeMaxima = a.QuantidadeMaxima,
                    });
            }

            UnitOfWork.Update(entidadeProduto);
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task<ProdutoDomain?> Obter(Guid idProduto) =>
            await UnitOfWork.Produtos
                .Where(p => p.Id == idProduto)
                .Include(p => p.IdSituacaoNavigation)
                .Include(p => p.IdTipoNavigation)
                .Include(p => p.IdCategoriaNavigation)
                .Include(p => p.AdicionalProdutos).ThenInclude(a => a.IdAdicionalNavigation).ThenInclude(a => a.IdSituacaoNavigation)
                .Select(p => ToDomain(p))
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<ProdutoDomain>> ObterTodos(Guid idEstabelecimento, CategoriaDeProduto? categoria)
        {
            var query = UnitOfWork.Produtos
                .Where(p => p.IdEstabelecimento == idEstabelecimento);

            if (categoria != null)
                query = query.Where(p => p.IdCategoria.Equals(categoria.Id));

            return await query
                .Include(p => p.IdTipoNavigation)
                .Include(p => p.IdSituacaoNavigation)
                .Include(p => p.IdCategoriaNavigation)
                .Include(a => a.AdicionalProdutos).ThenInclude(a => a.IdAdicionalNavigation).ThenInclude(a => a.IdSituacaoNavigation)
                .Select(p => ToDomain(p))
                .ToListAsync();
        }

        private static ProdutoDomain ToDomain(Domain.Entitys.Produto entity) =>
                    new(
                        entity.Id,
                        entity.IdEstabelecimento,
                        new SituacaoDeProdutoDomain(entity.IdSituacao, entity.IdSituacaoNavigation.Descricao),
                        new CategoriaDeProduto(entity.IdCategoria, entity.IdCategoriaNavigation.Descricao),
                        new TipoDoProdutoDomain(entity.IdTipo, entity.IdTipoNavigation.Descricao),
                        entity.Nome,
                        entity.Descricao,
                        entity.Preco,
                        entity.MinutosParaRetirada,
                        entity.Imagem,
                        entity.QtdeVezesVendido,
                        entity.NotaMedia.GetValueOrDefault(),
                        entity.QtdeVotos,
                        entity.AdicionalProdutos.Select(a =>
                        new AdicionalDoProdutoDomain(
                            a.IdAdicional,
                            a.IdAdicionalNavigation.IdEstabelecimento,
                            new SituacaoDeProdutoDomain(a.IdAdicionalNavigation.IdSituacao, a.IdAdicionalNavigation.IdSituacaoNavigation.Descricao),
                            a.IdAdicionalNavigation.Nome,
                            a.IdAdicionalNavigation.Preco,
                            a.QtdeMaxima
                            )).ToList()
                        );
    }
}
