using Dashdine.Domain.Domain.Cliente.EnderecoCliente;
using Dashdine.Domain.Interfafe;
using Dashdine.Domain.Entitys;
using Dashdine.Domain.Domain.Pedido;

namespace Dashdine.Domain.Interface.Cliente;

public interface IEnderecoClienteRepository : IBaseRepository<EnderecoCliente>
{
    Task Adicionar(EnderecoClienteDomain enderecoCliente);
    Task Atualizar(EnderecoClienteDomain enderecoCliente);
    Task<EnderecoClienteDomain?> Obter(Guid idEndereco);
    Task<LocalizacaoDoClienteDomain?> ObterLocalizacaoDoCliente(Guid idLocalizacaoCliente);
}
