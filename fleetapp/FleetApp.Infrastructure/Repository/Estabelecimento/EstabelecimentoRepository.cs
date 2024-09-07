using Dashdine.Domain.Domain.Cliente;
using Dashdine.Domain.Domain.Estabelecimento;
using Dashdine.Domain.Domain.Pedido.Cliente;
using Dashdine.Domain.Domain.Produto;
using Dashdine.Domain.Entitys;
using Dashdine.Domain.Interface.Estabelecimento;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dashdine.Infrastructure.Repository.Estabelecimento;

public class EstabelecimentoRepository(IConfiguration configuration) : BaseRepository<Domain.Entitys.Estabelecimento>(configuration), IEstabelecimentoRepository
{
    public async Task<string> ObterInformacaoUnicaJaCadastrada(EstabelecimentoDomain estabelecimento)
    {
        var estabelecimentoCadastrado = await UnitOfWork.Estabelecimentos.Where(e => e.Cnpj.Equals(estabelecimento.CNPJ)).FirstOrDefaultAsync();
        if (estabelecimentoCadastrado is null)
            return string.Empty;

        return estabelecimento.CNPJ;
    }

    public async Task Cadastrar(Guid idGestor, EstabelecimentoDomain estabelecimento)
    {
        if (estabelecimento.Endereco == null)
            throw new Exception("Informe o endereço do estabelecimento.");

        Gestor entidadeGestor = await UnitOfWork.Gestors.Where(g => g.Id == idGestor).FirstAsync();
        Domain.Entitys.Estabelecimento entidade = new()
        {
            Id = estabelecimento.Id,
            NomeFantasia = estabelecimento.NomeFantasia,
            Cnpj = estabelecimento.CNPJ,
            DataHoraCadastro = estabelecimento.DataHoraCadastro,
            IdSituacao = estabelecimento.Situacao.Id,
            RazaoSocial = estabelecimento.RazaoSocial,
            Telefone = estabelecimento.Telefone,
            Logo = estabelecimento.Logo,
        };
        entidade.Gestors.Add(entidadeGestor);
        UnitOfWork.Estabelecimentos.Add(entidade);

        Domain.Entitys.EnderecoEstabelecimento enderecoRetirada = new()
        {
            Id = estabelecimento.Endereco.Id,
            IdEstabelecimento = estabelecimento.Id,
            Cep = estabelecimento.Endereco.Cep,
            Logradouro = estabelecimento.Endereco.Logradouro,
            Bairro = estabelecimento.Endereco.Bairro,
            Cidade = estabelecimento.Endereco.Cidade,
            Estado = estabelecimento.Endereco.Estado,
            Complemento = estabelecimento.Endereco.Complemento,
            Numero = estabelecimento.Endereco.Numero,
            Latitude = estabelecimento.Endereco.Latitude,
            Longitude = estabelecimento.Endereco.Longitude,
            Timezone = estabelecimento.Endereco.TimeZone
        };

        UnitOfWork.EnderecoEstabelecimentos.Add(enderecoRetirada);
        await SaveChangesAsync();
    }

    public async Task<InformacoesDoEstabelecimento?> ObterInformacoes(Guid idEstabelecimento, DateTime dataInicioPedidos)
    {
        var estabelecimento = await UnitOfWork.Estabelecimentos
            .Include(e => e.HorariosFuncionamentos)
            .Include(e => e.EnderecoEstabelecimentos)
            .FirstOrDefaultAsync(e => e.Id == idEstabelecimento);

        if (estabelecimento is null) return null;

        var pedidos = await UnitOfWork.Pedidos
            .Where(pedido => pedido.DataHoraARetirar != null && pedido.DataHoraARetirar!.Value.Date >= dataInicioPedidos.Date)
            .Include(p => p.ProdutoPedidos).ThenInclude(produtoPedido => produtoPedido.IdProdutoNavigation)
            .Include(p => p.IdClienteNavigation)
            .Include(p => p.IdSituacaoNavigation)
            .ToListAsync();

        EnderecoRetirada? endereco = null;
        if (estabelecimento.EnderecoEstabelecimentos.Count != 0)
        {
            var entidadeEndereco = estabelecimento.EnderecoEstabelecimentos.First();
            endereco = new EnderecoRetirada(
                        entidadeEndereco.Id,
                        entidadeEndereco.Cep,
                        entidadeEndereco.Bairro,
                        entidadeEndereco.Estado,
                        entidadeEndereco.Cidade,
                        entidadeEndereco.Logradouro,
                        entidadeEndereco.Numero,
                        entidadeEndereco.Complemento,
                        entidadeEndereco.Latitude,
                        entidadeEndereco.Longitude,
                        entidadeEndereco.Timezone);
        }

        int quantidadeDeProdutosDoEstabelecimento = await UnitOfWork.Produtos.CountAsync(p => p.IdEstabelecimento == idEstabelecimento);
        return new InformacoesDoEstabelecimento()
        {
            Id = estabelecimento.Id,
            NomeFantasia = estabelecimento.NomeFantasia,
            EnderecoCompleto = endereco != null ? endereco.Completo : string.Empty,
            InformacoesSobreOsProdutos = new InformacoesSobreOsProdutos()
            {
                QuantidadeDeProdutosCadastrados = quantidadeDeProdutosDoEstabelecimento,
            },
        };
    }

    public async Task<IEnumerable<EstabelecimentoParaClienteDomain>> ObterEstabelecimentosAtivosDoMunicipio(string estado, string cidade)
    {
        var estabelecimentos = await UnitOfWork.
            EnderecoEstabelecimentos
            .Where(e => e.IdEstabelecimentoNavigation.IdSituacaoNavigation.Ativo)
            .Where(e => e.Cidade.Contains(cidade) && e.Estado.Equals(estado) && e.IdEstabelecimentoNavigation != null)
            .Include(e => e.IdEstabelecimentoNavigation)
            .ToListAsync();

        return estabelecimentos
            .Select(e =>
            new EstabelecimentoParaClienteDomain(
                e.IdEstabelecimento,
                e.IdEstabelecimentoNavigation!.Logo,
                e.IdEstabelecimentoNavigation!.NomeFantasia!,
                null,
                null))
            .OrderBy(e => e.NomeFantasia);
    }

    public async Task<IEnumerable<ProdutoDomain>> ObterProdutosAtivosDoEstabelecimento(Guid idEstabelecimento) =>
        await UnitOfWork.Produtos
            .Include(p => p.IdTipoNavigation)
            .Include(p => p.IdCategoriaNavigation)
            .Include(p => p.IdSituacaoNavigation)
            .Include(p => p.AdicionalProdutos).ThenInclude(p => p.IdAdicionalNavigation).ThenInclude(p => p.IdSituacaoNavigation)
            .Where(p => p.IdEstabelecimento.Equals(idEstabelecimento) && p.IdSituacaoNavigation.Ativo)
            .Select(p =>
                new ProdutoDomain(
                    p.Id,
                    p.IdEstabelecimento,
                    new SituacaoDeProdutoDomain(p.IdSituacaoNavigation.Id, p.IdSituacaoNavigation.Descricao),
                    new CategoriaDeProduto(p.IdCategoria, p.IdCategoriaNavigation.Descricao),
                    new TipoDoProdutoDomain(p.IdTipo, p.IdTipoNavigation.Descricao),
                    p.Nome,
                    p.Descricao,
                    p.Preco,
                    p.MinutosParaRetirada,
                    p.Imagem,
                    p.QtdeVezesVendido,
                    p.NotaMedia,
                    p.QtdeVotos,
                    p.AdicionalProdutos
                        .Where(a => a.IdAdicionalNavigation.IdSituacaoNavigation.Ativo)
                        .Select(a => new AdicionalDoProdutoDomain(a.IdAdicional, a.IdAdicionalNavigation.IdEstabelecimento, new SituacaoDeProdutoDomain(a.IdAdicionalNavigation.IdSituacao, a.IdAdicionalNavigation.IdSituacaoNavigation.Descricao), a.IdAdicionalNavigation.Nome, a.IdAdicionalNavigation.Preco, a.QtdeMaxima))))
            .ToListAsync();

    public async Task<EstabelecimentoDoPedidoDoClienteDomain?> ObterEstabelecimentoDoCliente(Guid idEstabelecimento)
    {
        var estabelecimento = await UnitOfWork.Estabelecimentos
            .Include(e => e.EnderecoEstabelecimentos)
            .FirstOrDefaultAsync(e => e.Id.Equals(idEstabelecimento));
        if (estabelecimento is null || estabelecimento.EnderecoEstabelecimentos.Count == 0) return null;
        var endereco = estabelecimento.EnderecoEstabelecimentos.First();
        return new(
            estabelecimento.Id,
            estabelecimento.Logo,
            estabelecimento.NomeFantasia,
            new Domain.Domain.Pedido.Cliente.EnderecoEstabelecimento(endereco.Id, endereco.Cep, endereco.Logradouro, endereco.Numero, endereco.Complemento, endereco.Bairro, endereco.Cidade, endereco.Estado, endereco.Latitude, endereco.Longitude, endereco.Timezone));
    }
}
