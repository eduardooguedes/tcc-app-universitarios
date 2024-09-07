using Dashdine.Domain.Domain.Cliente.Cartao;
using Dashdine.Domain.Entitys;
using Dashdine.Domain.Interface.Cliente;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dashdine.Infrastructure.Repository.Cliente;

public class CartoesClienteRepository(IConfiguration configuration) : BaseRepository<CartaoCliente>(configuration), ICartoesClienteRepository
{
    public async Task<IEnumerable<CartaoDomain>> ObterLista(Guid idCliente) =>
        await UnitOfWork.CartaoClientes
            .Where(c => c.IdCliente.Equals(idCliente))
            .Select(c => ToDomain(c))
            .ToListAsync();

    public async Task<CartaoDomain?> Obter(Guid idCartao)
    {
        return await UnitOfWork.CartaoClientes
           .AsQueryable()
           .AsNoTracking()
           .Where(c => c.Id.Equals(idCartao))
           .Select(c => ToDomain(c))
           .FirstOrDefaultAsync();
    }

    private static CartaoDomain ToDomain(CartaoCliente cartao) => new(cartao.Id, cartao.IdCliente, cartao.Apelido, cartao.FinalDoNumero, (EnumTipoDeCartao)cartao.IdTipo, cartao.Validade, cartao.Bandeira, cartao.IdEndereco);
    private static CartaoCliente FromDomain(CartaoDomain cartao) => new() { Id = cartao.Id, IdCliente = cartao.IdCliente, IdEndereco = cartao.IdEnderecoCobranca, Apelido = cartao.Apelido, Bandeira = cartao.Bandeira, FinalDoNumero = cartao.FinalDoNumero, IdTipo = (int)cartao.Tipo, Validade = cartao.Validade };

    public async Task AdicionarResumoDoCartao(CartaoDomain cartao)
    {
        await UnitOfWork.CartaoClientes.AddAsync(FromDomain(cartao));
        await SaveChangesAsync();
    }

    public async Task AtualizarCartao(CartaoDomain cartao)
    {
        UnitOfWork.CartaoClientes.Update(FromDomain(cartao));
        await SaveChangesAsync();
    }

    public async Task Excluir(Guid idCartao)
    {
        UnitOfWork.CartaoClientes.Remove(UnitOfWork.CartaoClientes.First(c => c.Id.Equals(idCartao)));
        await SaveChangesAsync();
    }
}
