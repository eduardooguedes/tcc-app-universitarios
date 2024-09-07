using Dashdine.Service.Models.Cliente;

namespace Dashdine.Service.Interface.Cliente;

public interface IClienteService
{
    Task<ProjecaoDeClienteLogado> Novo(DtoDeCliente cliente);
    Task Editar(Guid idUsuario, DtoDeCliente dtoCliente);
}
