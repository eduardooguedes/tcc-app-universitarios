using Dashdine.CrossCutting.Enums;
using Dashdine.Domain.Domain.Cliente.EnderecoCliente;
using Dashdine.Domain.Domain.Pedido;
using Dashdine.Domain.Interface.Cliente;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dashdine.Infrastructure.Repository.Cliente;

public class EnderecoClienteRepository(IConfiguration configuration) : BaseRepository<Domain.Entitys.EnderecoCliente>(configuration), IEnderecoClienteRepository
{
    public async Task Adicionar(EnderecoClienteDomain enderecoCliente)
    {
        var endereco = new Domain.Entitys.EnderecoCliente()
        {
            Id = enderecoCliente.Id,
            IdCliente = enderecoCliente.IdCliente,
            IdTipoEndereco = enderecoCliente.TipoDeEndereco.Id,
            Apelido = enderecoCliente.Apelido,
            Principal = enderecoCliente.Principal,
            Cep = enderecoCliente.Cep,
            Bairro = enderecoCliente.Bairro,
            Logradouro = enderecoCliente.Logradouro,
            Numero = enderecoCliente.Numero,
            Cidade = enderecoCliente.Cidade,
            Estado = enderecoCliente.Estado,
            Complemento = enderecoCliente.Complemento,
            Latitude = enderecoCliente.Latitude,
            Longitude = enderecoCliente.Longitude,
            Timezone = enderecoCliente.Timezone
        };

        await UnitOfWork.EnderecoClientes.AddAsync(endereco);
        await SaveChangesAsync();

        if (enderecoCliente.Principal)
        {
            var enderecosParaAtualizar = await UnitOfWork.EnderecoClientes
                .Where(e =>
                        e.IdCliente == enderecoCliente.IdCliente &&
                        e.Id != endereco.Id &&
                        e.Principal &&
                        e.IdTipoEndereco == endereco.IdTipoEndereco)
                .ToListAsync();

            if (enderecosParaAtualizar.Count > 0)
            {
                enderecosParaAtualizar.ForEach(e => e.Principal = false);
                await SaveChangesAsync();
            }
        }
    }

    public async Task Atualizar(EnderecoClienteDomain enderecoCliente)
    {
        var localizacao = await UnitOfWork.EnderecoClientes.FirstOrDefaultAsync(e => e.Id.Equals(enderecoCliente.Id));
        if (localizacao is null) return;

        localizacao.Apelido = enderecoCliente.Apelido;
        localizacao.Principal = enderecoCliente.Principal;
        localizacao.Cep = enderecoCliente.Cep;
        localizacao.Logradouro = enderecoCliente.Logradouro;
        localizacao.Numero = enderecoCliente.Numero;
        localizacao.Cidade = enderecoCliente.Cidade;
        localizacao.Estado = enderecoCliente.Estado;
        localizacao.Bairro = enderecoCliente.Bairro;
        localizacao.Longitude = enderecoCliente.Longitude;
        localizacao.Latitude = enderecoCliente.Latitude;
        localizacao.Timezone = enderecoCliente.Timezone;
        localizacao.Complemento = enderecoCliente.Complemento;

        UnitOfWork.Update(localizacao);
        await SaveChangesAsync();

        if (enderecoCliente.Principal)
        {
            var enderecosParaAtualizar = await UnitOfWork.EnderecoClientes
                .Where(e =>
                        e.IdCliente == enderecoCliente.IdCliente &&
                        e.Id != enderecoCliente.Id &&
                        e.Principal &&
                        e.IdTipoEndereco == enderecoCliente.TipoDeEndereco.Id)
                .ToListAsync();

            if (enderecosParaAtualizar.Count > 0)
            {
                enderecosParaAtualizar.ForEach(e => e.Principal = false);
                await SaveChangesAsync();
            }
        }
    }

    public Task<EnderecoClienteDomain?> Obter(Guid idEndereco) =>
        UnitOfWork.EnderecoClientes
        .Where(e => e.Id.Equals(idEndereco))
        .Include(t => t.IdTipoEnderecoNavigation)
        .Select(endereco =>
            new EnderecoClienteDomain(
                endereco.Id,
                endereco.IdCliente,
                new TipoEnderecoClienteDomain(endereco.IdTipoEndereco, endereco.IdTipoEnderecoNavigation.Descricao, endereco.IdTipoEnderecoNavigation.Cobranca),
                endereco.Apelido,
                endereco.Principal,
                endereco.Cep,
                endereco.Logradouro,
                endereco.Numero,
                endereco.Complemento,
                endereco.Bairro,
                endereco.Cidade,
                endereco.Estado,
                endereco.Latitude,
                endereco.Longitude,
                endereco.Timezone))
        .FirstOrDefaultAsync();

    public async Task<LocalizacaoDoClienteDomain?> ObterLocalizacaoDoCliente(Guid idLocalizacaoCliente)
        => await UnitOfWork.EnderecoClientes
            .Where(l => l.Id == idLocalizacaoCliente)
            .Select(l => new LocalizacaoDoClienteDomain(l.Id, l.Apelido, l.Latitude, l.Longitude, l.Timezone))
            .FirstOrDefaultAsync();
}