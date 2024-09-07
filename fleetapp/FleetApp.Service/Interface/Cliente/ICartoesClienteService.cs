using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Service.Models.Cliente;

namespace Dashdine.Service.Interface.Cliente;

public interface ICartoesClienteService
{
    Task<IEnumerable<ProjecaoDeCartaoDoCliente>> ObterLista(UsuarioAutenticado cliente);
    Task<ProjecaoDeCartaoDoCliente> Adicionar(UsuarioAutenticado usuarioAutenticado, DtoDeCartaoDoCliente dto);
    Task Atualizar(UsuarioAutenticado usuarioAutenticado, Guid idCartao, DtoDeCartaoDoClienteEdicao dto);
    Task Excluir(UsuarioAutenticado usuarioAutenticado, Guid idCartao);
}
