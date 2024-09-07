using Dashdine.Domain.Domain.Produto;
using Dashdine.Domain.Interface.Produto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dashdine.Infrastructure.Repository.Produto;

public class AdicionalRepository(IConfiguration configuration) : BaseRepository<Domain.Entitys.AdicionalProduto>(configuration), IAdicionalRepository
{
    public async Task<Guid> Cadastrar(Adicional adicional)
    {
        if (string.IsNullOrEmpty(adicional.Nome))
            throw new Exception("Informe o nome do adicional.");

        if (adicional.Preco == null)
            throw new Exception("Informe o preço do adicional.");

        Domain.Entitys.Adicional entidade = new()
        {
            Id = adicional.Id,
            Nome = adicional.Nome,
            Preco = adicional.Preco.Value,
            IdEstabelecimento = adicional.IdEstabelecimento,
            IdSituacao = adicional.Situacao.Id
        };

        UnitOfWork.Adicionals.Add(entidade);
        await UnitOfWork.SaveChangesAsync();
        return entidade.Id;
    }

    public async Task Atualizar(Adicional adicional)
    {
        if (string.IsNullOrEmpty(adicional.Nome)) throw new Exception("Informe o nome do adicional.");

        if (adicional.Preco == null) throw new Exception("Informe o preço do adicional.");

        var entidade = await UnitOfWork.Adicionals.FirstOrDefaultAsync(a => a.Id == adicional.Id) ?? throw new Exception("Adicional não encontrado.");

        entidade.Nome = adicional.Nome;
        entidade.Preco = adicional.Preco.Value;
        entidade.IdSituacao = adicional.Situacao.Id;

        UnitOfWork.Adicionals.Update(entidade);
        await UnitOfWork.SaveChangesAsync();
    }

    public async Task<Adicional?> ObterPorId(Guid id)
    {
        var adicional = await UnitOfWork.Adicionals
            .Include(a => a.IdSituacaoNavigation)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (adicional is null)
            return null;

        return new(adicional.Id, adicional.IdEstabelecimento, new(adicional.IdSituacao, adicional.IdSituacaoNavigation.Descricao), adicional.Nome, adicional.Preco);
    }

    public async Task<List<ProdutoDomain>> ObterProdutosVinculadosOuException(Guid id)
    {
        var adicionaisEProdutos = await UnitOfWork.AdicionalProdutos
            .Where(a => a.IdAdicional == id)
            .Include(p => p.IdProdutoNavigation).ThenInclude(p => p.IdCategoriaNavigation)
            .Include(p => p.IdProdutoNavigation).ThenInclude(p => p.IdTipoNavigation)
            .Include(p => p.IdProdutoNavigation).ThenInclude(p => p.IdSituacaoNavigation)
            .ToListAsync();

        if (adicionaisEProdutos.Count == 0)
            throw new Exception("Adicional não possui produto vinculado.");

        return adicionaisEProdutos
            .ConvertAll(
                p =>
                new ProdutoDomain(
                    p.IdProdutoNavigation.Id,
                    p.IdProdutoNavigation.IdEstabelecimento,
                    new SituacaoDeProdutoDomain(p.IdProdutoNavigation.IdSituacao, p.IdProdutoNavigation.IdSituacaoNavigation.Descricao),
                    new CategoriaDeProduto(p.IdProdutoNavigation.IdCategoria, p.IdProdutoNavigation.IdCategoriaNavigation.Descricao),
                    new TipoDoProdutoDomain(p.IdProdutoNavigation.IdTipo, p.IdProdutoNavigation.IdTipoNavigation.Descricao),
                    p.IdProdutoNavigation.Nome,
                    p.IdProdutoNavigation.Descricao,
                    p.IdProdutoNavigation.Preco,
                    p.IdProdutoNavigation.MinutosParaRetirada,
                    p.IdProdutoNavigation.Imagem,
                    p.IdProdutoNavigation.QtdeVezesVendido,
                    p.IdProdutoNavigation.NotaMedia.GetValueOrDefault(),
                    p.IdProdutoNavigation.QtdeVotos,
                    null
                    ));
    }

    public async Task<List<Adicional>> ObterTodos(Guid idEstabelecimento) =>
        await UnitOfWork.Adicionals
            .Where(p => p.IdEstabelecimento == idEstabelecimento)
            .Include(a => a.AdicionalProdutos)
            .Include(a => a.IdSituacaoNavigation)
            .Select(a =>
                new Adicional(a.Id, a.IdEstabelecimento, new SituacaoDeProdutoDomain(a.IdSituacao, a.IdSituacaoNavigation.Descricao), a.Nome, a.Preco, a.AdicionalProdutos.Count))
            .ToListAsync();

    public async Task<List<Adicional>> ObterPorFiltro(string filtroDeNome, int quantidadeDeAdicionaisRetornados) =>
        await UnitOfWork.Adicionals
            .Include(a => a.IdSituacaoNavigation)
            .Where(a => a.Nome.Contains(filtroDeNome, StringComparison.OrdinalIgnoreCase))
            .OrderBy(a => a.Nome)
            .Take(quantidadeDeAdicionaisRetornados)
            .Select(a => new Adicional(a.Id, a.IdEstabelecimento, new SituacaoDeProdutoDomain(a.IdSituacao, a.IdSituacaoNavigation.Descricao), a.Nome, a.Preco, null))
            .ToListAsync();

    public async Task RemoverVinculo(Guid idAdicional, Guid idProduto)
    {
        var vinculo = await UnitOfWork.AdicionalProdutos
            .FirstOrDefaultAsync(a => a.IdAdicional == idAdicional && a.IdProduto == idProduto)
            ?? throw new Exception("Adicional e produto não possuem vínculo.");

        UnitOfWork.AdicionalProdutos.Remove(vinculo);
        await UnitOfWork.SaveChangesAsync();
    }
}
