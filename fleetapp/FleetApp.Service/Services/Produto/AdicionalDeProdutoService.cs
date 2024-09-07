using Dashdine.Domain.Domain.Produto;
using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Domain.Interface.Estabelecimento;
using Dashdine.Domain.Interface.Produto;
using Dashdine.Service.Exceptions.Estabelecimento;
using Dashdine.Service.Exceptions.Gestor;
using Dashdine.Service.Interface.Produto;
using Dashdine.Service.Models.Produto;

namespace Dashdine.Service.Services.Produto;

public class AdicionalDeProdutoService(
    IAdicionalRepository adicionalRepository,
    IGestorRepository gestorRepository,
    ISituacaoProdutoRepository situacaoProdutoRepository) : IAdicionalDeProdutoService
{
    private readonly IAdicionalRepository adicionalRepository = adicionalRepository;
    private readonly IGestorRepository gestorRepository = gestorRepository;

    public async Task<ProjecaoDeAdicionalParaEdicao> GestorObterPorId(UsuarioAutenticado usuario, Guid idAdicional)
    {
        var gestor = await gestorRepository.ObterPorIdComEstabelecimento(usuario.Id) ?? throw new GestorNaoEncontradoException();

        var adicional = await adicionalRepository.ObterPorId(idAdicional) ?? throw new AdicionalNaoEncontradoException();

        if (adicional.IdEstabelecimento != gestor!.Estabelecimento!.Id)
            throw new Exception("Adicional não encontrado.");

        var produtosVinculados = await adicionalRepository.ObterProdutosVinculadosOuException(idAdicional);

        if (produtosVinculados.Any(p => p.IdEstabelecimento != gestor.Estabelecimento.Id))
            produtosVinculados = null;

        return new ProjecaoDeAdicionalParaEdicao()
        {
            Id = adicional.Id.ToString(),
            Nome = adicional.Nome ?? string.Empty,
            Preco = adicional.Preco.GetValueOrDefault(),
            Situacao = adicional.Situacao,
            ProdutosQuePossuemAdicional =
                produtosVinculados?.Select(p => new ProjecaoParaListagemDeProduto()
                {
                    Id = p.Id.ToString(),
                    Categoria = p.Categoria,
                    Tipo = p.Tipo,
                    Descricao = p.Descricao,
                    Nome = p.Nome,
                    Preco = p.Preco,
                    Situacao = p.Situacao,
                    TempoEmMinutosParaRetirada = p.TempoEmMinutosParaRetirada
                })
                .OrderBy(p => p.Nome),
        };
    }

    public async Task<IEnumerable<ProjecaoDeAdicionalParaListagem>> GestorObterTodos(UsuarioAutenticado usuario)
    {
        var gestor = await gestorRepository.ObterPorIdComEstabelecimento(usuario.Id) ?? throw new GestorNaoPossuiEstabelecimentoException();
        return (await adicionalRepository.ObterTodos(gestor!.Estabelecimento!.Id))
            .Select(a => new ProjecaoDeAdicionalParaListagem(a.Id.ToString(), a.Nome, a.Preco, a.Situacao, a.QuantidadeDeProdutosVinculados ?? 0));
    }

    public async Task<Guid> Cadastrar(UsuarioAutenticado usuario, DtoDeAdicional dtoAdicional)
    {
        var gestor = await gestorRepository.ObterPorIdComEstabelecimento(usuario.Id) ?? throw new GestorNaoPossuiEstabelecimentoException();
        var situacaoAtivo = await situacaoProdutoRepository.ObterAtivo();
        return await adicionalRepository.Cadastrar(new Adicional(gestor!.Estabelecimento!.Id, situacaoAtivo, dtoAdicional.Nome, dtoAdicional.Preco));
    }

    public async Task Atualizar(UsuarioAutenticado usuario, Guid idAdicional, DtoDeAdicional dtoDeAdicional)
    {
        var gestor = await gestorRepository.ObterPorIdComEstabelecimento(usuario.Id) ?? throw new GestorNaoPossuiEstabelecimentoException();

        var adicional = await adicionalRepository.ObterPorId(idAdicional) ?? throw new AdicionalNaoEncontradoException();

        if (adicional.IdEstabelecimento != gestor.Estabelecimento!.Id)
            throw new AdicionalNaoEncontradoException();

        await adicionalRepository.Atualizar(new Adicional(adicional.Id, adicional.IdEstabelecimento, adicional.Situacao, dtoDeAdicional.Nome, dtoDeAdicional.Preco));
    }

    public async Task AtualizarSituacao(UsuarioAutenticado usuarioAutenticado, Guid idAdicional, int situacao)
    {
        var novaSituacao = await situacaoProdutoRepository.Obter(situacao) ?? throw new SituacaoDeProdutoNaoEncontradaException();

        var gestor = await gestorRepository.ObterPorIdComEstabelecimento(usuarioAutenticado.Id) ?? throw new GestorNaoPossuiEstabelecimentoException();

        var adicional = await adicionalRepository.ObterPorId(idAdicional) ?? throw new AdicionalNaoEncontradoException();

        if (adicional.IdEstabelecimento != gestor!.Estabelecimento!.Id)
            throw new AdicionalNaoEncontradoException();

        adicional.AtualizarSituacao(novaSituacao);
        await adicionalRepository.Atualizar(adicional);
    }

    public async Task<IEnumerable<ProjecaoDeAdicionalParaFiltro>> GestorObterPorFiltro(string filtroDeNome) =>
        (await adicionalRepository.ObterPorFiltro(filtroDeNome, 10))
            .Select(a => new ProjecaoDeAdicionalParaFiltro(a.Id.ToString(), a.Nome, a.Preco));

    public async Task RemoverVinculoEntreAdicionalEProduto(UsuarioAutenticado usuarioAutenticado, Guid idAdicional, Guid idProduto)
    {
        _ = await gestorRepository.ObterPorIdComEstabelecimento(usuarioAutenticado.Id) ?? throw new GestorNaoPossuiEstabelecimentoException();
        await adicionalRepository.RemoverVinculo(idAdicional, idProduto);
    }
}