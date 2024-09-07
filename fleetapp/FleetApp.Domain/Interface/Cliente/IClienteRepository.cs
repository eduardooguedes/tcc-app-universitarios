using Dashdine.Domain.Interfafe;

namespace Dashdine.Domain.Interface.Cliente;

public interface IClienteRepository : IBaseRepository<Entitys.Cliente>
{
    Task<string> ObterInformacaoUnicaJaCadastrada(Domain.Cliente.ClienteDomain cliente);
    Task Novo(Domain.Cliente.ClienteDomain cliente);
    Task<Domain.Cliente.ClienteDomain?> ObterPorEmail(string email);
}
