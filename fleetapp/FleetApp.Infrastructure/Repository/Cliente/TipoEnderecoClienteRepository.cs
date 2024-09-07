using Dashdine.CrossCutting.Enums;
using Dashdine.Domain.Domain.Cliente.EnderecoCliente;
using Dashdine.Domain.Entitys;
using Dashdine.Domain.Interface.Cliente;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dashdine.Infrastructure.Repository.Cliente;

public class TipoEnderecoClienteRepository(IConfiguration configuration) : BaseRepository<TipoEnderecoCliente>(configuration), ITipoEnderecoClienteRepository
{
    public async Task<TipoEnderecoClienteDomain?> Obter(int Id) =>
        await UnitOfWork.TipoEnderecoClientes
            .Where(t => t.Id == Id)
            .Select(t => new TipoEnderecoClienteDomain(t.Id, t.Descricao, t.Cobranca))
            .FirstOrDefaultAsync();
}
