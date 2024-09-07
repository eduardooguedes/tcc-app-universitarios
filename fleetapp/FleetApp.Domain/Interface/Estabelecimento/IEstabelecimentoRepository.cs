using Dashdine.Domain.Domain.Cliente;
using Dashdine.Domain.Domain.Estabelecimento;
using Dashdine.Domain.Domain.Pedido.Cliente;
using Dashdine.Domain.Entitys;
using Dashdine.Domain.Interfafe;

namespace Dashdine.Domain.Interface.Estabelecimento;

public interface IEstabelecimentoRepository : IBaseRepository<Entitys.Estabelecimento>
{
    Task<string> ObterInformacaoUnicaJaCadastrada(Domain.Estabelecimento.EstabelecimentoDomain estabelecimento);
    Task Cadastrar(Guid idGestor, Domain.Estabelecimento.EstabelecimentoDomain estabelecimento);
    Task<InformacoesDoEstabelecimento?> ObterInformacoes(Guid idEstabelecimento, DateTime dataInicioPedidos);
    Task<IEnumerable<EstabelecimentoParaClienteDomain>> ObterEstabelecimentosAtivosDoMunicipio(string estado, string cidade);
    Task<IEnumerable<Domain.Produto.ProdutoDomain>> ObterProdutosAtivosDoEstabelecimento(Guid idEstabelecimento);
    Task<EstabelecimentoDoPedidoDoClienteDomain?> ObterEstabelecimentoDoCliente(Guid idEstabelecimento);
}
