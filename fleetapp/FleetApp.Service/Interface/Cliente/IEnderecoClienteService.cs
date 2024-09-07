using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Service.Models.Cliente;

namespace Dashdine.Service.Interface.Cliente;

public interface IEnderecoClienteService
{
    Task<Guid> Adicionar(UsuarioAutenticado usuarioAutenticado, DtoDeEnderecoCliente dto);
    Task Atualizar(UsuarioAutenticado usuarioAutenticado, Guid idEndereco, DtoDeEnderecoCliente dto);
    Task<IEnumerable<ProjecaoDeEnderecoCliente>> ObterLista(UsuarioAutenticado usuarioAutenticado, int? idTipoEndereco);
    Task Remover(UsuarioAutenticado usuarioAutenticado, Guid idEndereco);
}
