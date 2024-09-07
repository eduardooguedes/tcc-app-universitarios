using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Service.Models.Cliente.Estabelecimento;
using Dashdine.Service.Models.Cliente.Produto;

namespace Dashdine.Service.Interface.Cliente;

public interface IEstabelecimentosClienteService
{
    Task<IEnumerable<ProjecaoDeEstabelecimentoParaCliente>> ObterEstabelecimentos(UsuarioAutenticado usuarioAutenticado, Guid idLocalizacao);
    Task<IEnumerable<ProjecaoDeCategoriaDeProdutosDoEstabelecimentoParaCliente>> ObterProdutosDoEstabelecimento(UsuarioAutenticado usuarioAutenticado, Guid idEstabelecimento);

}
