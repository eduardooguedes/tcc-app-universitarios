using Dashdine.Domain.Domain.Cliente.EnderecoCliente;
using Dashdine.Domain.Entitys;
using Dashdine.Domain.Interfafe;

namespace Dashdine.Domain.Interface.Cliente;

public interface ITipoEnderecoClienteRepository : IBaseRepository<TipoEnderecoCliente>
{
    Task<TipoEnderecoClienteDomain?> Obter(int Id);
}
