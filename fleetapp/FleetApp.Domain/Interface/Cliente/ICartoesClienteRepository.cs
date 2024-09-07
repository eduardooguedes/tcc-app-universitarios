using Dashdine.Domain.Domain.Cliente.Cartao;
using Dashdine.Domain.Interfafe;

namespace Dashdine.Domain.Interface.Cliente;

public interface ICartoesClienteRepository : IBaseRepository<Entitys.CartaoCliente>
{
    Task<IEnumerable<CartaoDomain>> ObterLista(Guid idCliente);
    Task AdicionarResumoDoCartao(CartaoDomain cartao);
    Task<CartaoDomain?> Obter(Guid idCartao);
    Task AtualizarCartao(CartaoDomain cartao);
    Task Excluir(Guid idCartao);
}
