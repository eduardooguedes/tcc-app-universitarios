using Dashdine.CrossCutting.Enums.Usuario;
using Dashdine.Domain.Domain.Estabelecimento.Gestor;
using Dashdine.Domain.Domain.Produto;
using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Domain.Interface;
using Dashdine.Domain.Interface.Estabelecimento;
using Dashdine.Domain.Interface.Produto;
using Dashdine.Service.Exceptions;
using Dashdine.Service.Exceptions.Estabelecimento;
using Dashdine.Service.Exceptions.Gestor;
using Dashdine.Service.Exceptions.Usuario;
using Dashdine.Service.Extensions;
using Dashdine.Service.Interface.Produto;
using Dashdine.Service.Models.Produto;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Dashdine.Service.Services.Produto;

public class ProdutoService(
    IProdutoRepository produtoRepository,
    IGestorRepository gestorRepository,
    IImagemDoProdutoRepository imagemProdutoRepository,
    ICategoriaDeProdutoRepository categoriaDeProdutoRepository,
    ISituacaoProdutoRepository situacaoProdutoRepository,
    ITipoDeProdutoRepository tipoDeProdutoRepository,
    IParametroRepository parametroRepository) : IProdutoService
{
    public async Task AtualizarSituacao(UsuarioAutenticado usuarioAutenticado, Guid idProduto, int situacao)
    {
        var novaSituacao = await situacaoProdutoRepository.Obter(situacao) ?? throw new SituacaoDeProdutoNaoEncontradaException();

        var gestor = await gestorRepository.ObterPorIdComEstabelecimento(usuarioAutenticado.Id) ?? throw new GestorNaoPossuiEstabelecimentoException();

        var produto = await produtoRepository.Obter(idProduto) ?? throw new ProdutoNaoEncontradoException();
        if (produto.IdEstabelecimento != gestor.Estabelecimento!.Id)
            throw new ProdutoNaoEncontradoException();

        produto.AtualizarSituacao(novaSituacao);
        await produtoRepository.Atualizar(produto);
    }

    public async Task<ProjecaoParaListagemDeProduto> Cadastrar(UsuarioAutenticado usuarioAutenticado, DtoDeProduto dto)
    {
        var gestor = await gestorRepository.ObterPorIdComEstabelecimento(usuarioAutenticado.Id) ?? throw new GestorNaoPossuiEstabelecimentoException();

        List<AdicionalDoProdutoDomain> adicionais = [];
        if (dto.Adicionais?.Count > 0)
        {
            foreach (DtoDeAdicionalDoProduto adicional in dto.Adicionais)
            {
                adicionais.Add(new AdicionalDoProdutoDomain(adicional.Id, gestor.Estabelecimento!.Id, adicional.QuantidadeMaxima));
            }
        }

        var tipoDoProduto = await tipoDeProdutoRepository.Obter(dto.IdTipoProduto) ?? throw new TipoDoProdutoNaoExisteException();
        var categoriaDoProduto = await categoriaDeProdutoRepository.Obter(dto.IdCategoria) ?? throw new CategoriaDoProdutoNaoDisponivelException();
        var situacaoAtiva = await situacaoProdutoRepository.ObterAtivo();

        var produto = new ProdutoDomain(gestor.Estabelecimento!.Id, situacaoAtiva, categoriaDoProduto, tipoDoProduto, dto.Nome, dto.Descricao, dto.Preco, dto.TempoEmMinutosParaRetirada, await parametroRepository.ObterImagemPadraoProduto(), adicionais);
        await produtoRepository.Cadastrar(produto);

        return new ProjecaoParaListagemDeProduto()
        {
            Id = produto.Id.ToString(),
            Categoria = produto.Categoria,
            Tipo = produto.Tipo,
            Descricao = produto.Descricao,
            Nome = produto.Nome,
            Imagem = produto.Imagem,
            Preco = produto.Preco,
            Situacao = produto.Situacao,
            TempoEmMinutosParaRetirada = produto.TempoEmMinutosParaRetirada
        };
    }

    public async Task<string> AtualizarImagem(UsuarioAutenticado usuarioAutenticado, Guid idProduto, IFormFile imagem)
    {
        var gestor = await gestorRepository.ObterPorIdComEstabelecimento(usuarioAutenticado.Id) ?? throw new GestorNaoPossuiEstabelecimentoException();

        var produto = await produtoRepository.Obter(idProduto) ?? throw new ProdutoNaoEncontradoException();

        if (produto.IdEstabelecimento != gestor.Estabelecimento!.Id)
            throw new ProdutoNaoEncontradoException();

        try { produto.Imagem = await imagemProdutoRepository.AtualizarImagem(gestor.Estabelecimento.Id, produto.Id, imagem); } catch (Exception ex) { throw new ServiceException(ex.Message); }
        await produtoRepository.Atualizar(produto);
        return produto.Imagem;
    }

    public async Task<ProjecaoParaListagemDeProduto> Atualizar(UsuarioAutenticado usuarioAutenticado, Guid idProduto, DtoDeProduto dtoProduto)
    {
        Gestor gestor = await gestorRepository.ObterPorIdComEstabelecimento(usuarioAutenticado.Id) ?? throw new GestorNaoPossuiEstabelecimentoException();

        var produto = await produtoRepository.Obter(idProduto) ?? throw new ProdutoNaoEncontradoException();

        if (produto.IdEstabelecimento != gestor.Estabelecimento!.Id)
            throw new ProdutoNaoEncontradoException();

        produto.Categoria = await categoriaDeProdutoRepository.Obter(dtoProduto.IdCategoria) ?? throw new CategoriaDoProdutoNaoDisponivelException();
        produto.Tipo = await tipoDeProdutoRepository.Obter(dtoProduto.IdTipoProduto) ?? throw new TipoDoProdutoNaoExisteException();
        produto.Nome = dtoProduto.Nome;
        produto.Descricao = dtoProduto.Descricao;
        produto.Preco = dtoProduto.Preco;
        produto.TempoEmMinutosParaRetirada = dtoProduto.TempoEmMinutosParaRetirada;

        List<AdicionalDoProdutoDomain> adicionaisParaDesvincular = [];
        List<DtoDeAdicionalDoProduto> dtoAdicionaisAtualizados = [];

        if (produto.Adicionais != null)
        {
            foreach (AdicionalDoProdutoDomain adicional in produto.Adicionais)
            {
                DtoDeAdicionalDoProduto? dtoAdicional = dtoProduto.Adicionais?.FirstOrDefault(a => a.Id == adicional.Id);
                if (dtoAdicional != null)
                {
                    adicional.QuantidadeMaxima = dtoAdicional.QuantidadeMaxima;
                    dtoAdicionaisAtualizados.Add(dtoAdicional);
                }
                else
                {
                    adicionaisParaDesvincular.Add(adicional);
                }
            }
        }
        else
        {
            produto.Adicionais = [];
        }

        if (dtoProduto.Adicionais?.Count > 0)
        {
            foreach (DtoDeAdicionalDoProduto dtoAdicional in dtoProduto.Adicionais)
            {
                if (!dtoAdicionaisAtualizados.Contains(dtoAdicional))
                {
                    produto.Adicionais.Append(new AdicionalDoProdutoDomain(dtoAdicional.Id, gestor.Estabelecimento!.Id, dtoAdicional.QuantidadeMaxima));
                }
            }
        }

        adicionaisParaDesvincular.ForEach(a => produto.Adicionais.Remove(a));
        await produtoRepository.Atualizar(produto);

        return new ProjecaoParaListagemDeProduto()
        {
            Id = produto.Id.ToString(),
            Categoria = produto.Categoria,
            Tipo = produto.Tipo,
            Descricao = produto.Descricao,
            Nome = produto.Nome,
            Imagem = produto.Imagem,
            Preco = produto.Preco,
            Situacao = produto.Situacao,
            TempoEmMinutosParaRetirada = produto.TempoEmMinutosParaRetirada
        };
    }

    public async Task<ProjecaoDeProduto> ObterPorId(UsuarioAutenticado usuarioAutenticado, Guid idProduto)
    {
        var gestor = await gestorRepository.ObterPorIdComEstabelecimento(usuarioAutenticado.Id) ?? throw new GestorNaoPossuiEstabelecimentoException();

        var produto = await produtoRepository.Obter(idProduto) ?? throw new ProdutoNaoEncontradoException();

        if (produto.IdEstabelecimento != gestor.Estabelecimento!.Id)
            throw new ProdutoNaoEncontradoException();

        return new ProjecaoDeProduto()
        {
            Id = produto.Id.ToString(),
            Categoria = produto.Categoria,
            Descricao = produto.Descricao,
            Nome = produto.Nome,
            NotaMedia = produto.NotaMedia,
            Imagem = produto.Imagem,
            Preco = produto.Preco,
            Situacao = produto.Situacao.AsProjecaoDeSituacaoDoProduto(),
            QuantidadeVezesPreparado = produto.QuantidadeVezesVendido,
            TempoEmMinutosParaRetirada = produto.TempoEmMinutosParaRetirada,
            Tipo = produto.Tipo,
            Adicionais = produto.Adicionais?
                .Select(a => new ProjecaoDeAdicionalDeProduto()
                {
                    Id = a.Id.ToString(),
                    Nome = a.Nome ?? string.Empty,
                    Situacao = a.Situacao,
                    PrecoUnitario = a.Preco.GetValueOrDefault(),
                    QuantidadeMaxima = a.QuantidadeMaxima
                })
                .ToList(),
        };
    }

    public async Task<IEnumerable<ProjecaoParaListagemDeProduto>> GestorObterTodos(UsuarioAutenticado usuarioAutenticado, int? idCategoria)
    {
        var gestor = await gestorRepository.ObterPorIdComEstabelecimento(usuarioAutenticado.Id) ?? throw new GestorNaoPossuiEstabelecimentoException();

        CategoriaDeProduto? categoria = null;
        if (idCategoria.HasValue)
            categoria = await categoriaDeProdutoRepository.Obter(idCategoria.Value) ?? throw new CategoriaDoProdutoNaoDisponivelException();

        return (await produtoRepository.ObterTodos(gestor!.Estabelecimento!.Id, categoria))
            .Select(p => new ProjecaoParaListagemDeProduto()
            {
                Id = p.Id.ToString(),
                Categoria = p.Categoria,
                Tipo = p.Tipo,
                Descricao = p.Descricao,
                Nome = p.Nome,
                Imagem = p.Imagem,
                Preco = p.Preco,
                Situacao = p.Situacao,
                TempoEmMinutosParaRetirada = p.TempoEmMinutosParaRetirada
            })
            .OrderBy(p => p.Nome);
    }

    public async Task RemoverImagem(UsuarioAutenticado usuarioAutenticado, Guid idProduto)
    {
        if (!usuarioAutenticado.TipoDeUsuario.Equals(EnumTipoDeUsuario.Gestor))
            throw new UsuarioNaoEncontradoException();

        var gestor = await gestorRepository.ObterPorIdComEstabelecimento(usuarioAutenticado.Id) ?? throw new EstabelecimentoNaoEncontradoException();

        var produto = await produtoRepository.Obter(idProduto) ?? throw new ProdutoNaoEncontradoException();

        if (produto.IdEstabelecimento != gestor.Estabelecimento!.Id)
            throw new ProdutoNaoEncontradoException();

        try { await imagemProdutoRepository.RemoverImagem(gestor.Estabelecimento.Id, idProduto); } catch (Exception ex) { throw new ServiceException(ex.Message); }

        produto.Imagem = await parametroRepository.ObterImagemPadraoProduto();
        await produtoRepository.Atualizar(produto);
    }
}
