using Dashdine.CrossCutting.Enums.Usuario;
using Dashdine.Domain.Domain.Cliente.EnderecoCliente;
using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Domain.Interface.Cliente;
using Dashdine.Domain.Interface.Estabelecimento;
using Dashdine.Service.Exceptions.Cliente;
using Dashdine.Service.Extensions;
using Dashdine.Service.Interface.Cliente;
using Dashdine.Service.Models.Cliente.Estabelecimento;
using Dashdine.Service.Models.Cliente.Produto;

namespace Dashdine.Service.Services.Cliente;

public sealed class EstabelecimentosClienteService(IEstabelecimentoRepository estabelecimentoRepository, IEnderecoClienteRepository enderecoClienteRepository) : IEstabelecimentosClienteService
{
    public async Task<IEnumerable<ProjecaoDeEstabelecimentoParaCliente>> ObterEstabelecimentos(UsuarioAutenticado usuarioAutenticado, Guid idLocalizacao)
    {
        var enderecoCliente = await enderecoClienteRepository.Obter(idLocalizacao) ?? throw new LocalizacaoDoClienteNaoEncontradaException();

        if (!enderecoCliente.IdCliente.Equals(usuarioAutenticado.Id))
            throw new LocalizacaoDoClienteNaoEncontradaException();

        var estabelecimentosProximos = await ObterEstabelecimentosAtivosProximos(enderecoCliente);
        return estabelecimentosProximos.Any()
            ? estabelecimentosProximos
            : (await estabelecimentoRepository.ObterEstabelecimentosAtivosDoMunicipio(enderecoCliente.Estado, enderecoCliente.Cidade))
            .Select(e => e.AsProjecaoDeEstabelecimentoParaCliente())
            .ToList();
    }

    private async Task<IEnumerable<ProjecaoDeEstabelecimentoParaCliente>> ObterEstabelecimentosAtivosProximos(EnderecoClienteDomain localizacao) =>
        (await estabelecimentoRepository.ObterEstabelecimentosAtivosProximos(localizacao.Latitude, localizacao.Longitude, localizacao.Timezone))
            .Select(e => e.AsProjecaoDeEstabelecimentoParaCliente())
    .ToList();

    public async Task<IEnumerable<ProjecaoDeCategoriaDeProdutosDoEstabelecimentoParaCliente>> ObterProdutosDoEstabelecimento(UsuarioAutenticado usuarioAutenticado, Guid idEstabelecimento)
    {
        if (!usuarioAutenticado.TipoDeUsuario.Equals(EnumTipoDeUsuario.Cliente))
            throw new ClienteNaoEncontradoException();

        var produtosPorCategoria = new List<ProjecaoDeCategoriaDeProdutosDoEstabelecimentoParaCliente>();
        var produtos = await estabelecimentoRepository.ObterProdutosAtivosDoEstabelecimento(idEstabelecimento);
        produtos
            .Select(p => p.Categoria)
            .Distinct()
            .OrderBy(p => p.Id)
            .ToList()
            .ForEach(categoria => produtosPorCategoria.Add(
                    new(categoria.Id,
                        categoria.Descricao,
                        produtos
                            .Where(p => p.Categoria.Id.Equals(categoria.Id))
                            .OrderBy(p => p.Preco)
                            .ThenBy(p => p.Nome)
                            .Select(p => p.AsProdutoDoEstabelecimentoParaCliente())
                            .AsEnumerable())));
        return produtosPorCategoria;
    }
}
