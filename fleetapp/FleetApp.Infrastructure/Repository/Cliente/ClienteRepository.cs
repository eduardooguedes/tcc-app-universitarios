using Dashdine.Domain.Domain.Cliente;
using Dashdine.Domain.Interface.Cliente;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dashdine.Infrastructure.Repository.Cliente;

public class ClienteRepository(IConfiguration configuration) : BaseRepository<Domain.Entitys.Cliente>(configuration), IClienteRepository
{
    public async Task<string> ObterInformacaoUnicaJaCadastrada(ClienteDomain cliente)
    {
        var clienteCadastrado = await UnitOfWork.Clientes.Where(c => c.Cpf.Equals(cliente.CPF) || c.Email.Equals(cliente.Email) || c.Celular.Equals(cliente.Celular)).FirstOrDefaultAsync();
        if (clienteCadastrado is null)
            return string.Empty;

        if (clienteCadastrado.Email.Equals(cliente.Email))
            return cliente.Email;

        if (clienteCadastrado.Cpf.Equals(cliente.CPF))
            return cliente.CPF;

        return cliente.Celular;
    }

    public async Task Novo(ClienteDomain cliente)
    {
        var entidadeCliente = new Domain.Entitys.Cliente()
        {
            Id = cliente.Id,
            IdSituacao = cliente.Situacao.Id,
            DataHoraCadastro = cliente.DataHoraCadastro,
            Nome = cliente.Nome,
            Sobrenome = cliente.Sobrenome,
            Cpf = cliente.CPF,
            DataNascimento = cliente.DataDeNascimento,
            Email = cliente.Email,
            EmailConfirmado = cliente.EmailConfirmado,
            Celular = cliente.Celular,
            CelularConfirmado = cliente.CelularConfirmado,
            Senha = cliente.SenhaCriptografada,
        };

        await UnitOfWork.Clientes.AddAsync(entidadeCliente);
        await SaveChangesAsync();
    }

    public async Task<ClienteDomain?> ObterPorEmail(string email)
    {
        return await UnitOfWork.Clientes
            .AsNoTracking()
            .Where(c => c.Email == email)
            .Select(c => new ClienteDomain(
                c.Id, SituacaoDeCliente.ObterPorIdOuNovo(c.IdSituacao), c.DataHoraCadastro, c.Nome, c.Sobrenome, c.Cpf, c.DataNascimento,
                c.Email, c.EmailConfirmado, c.Celular, c.CelularConfirmado, c.Senha))
            .FirstOrDefaultAsync();
    }
}
