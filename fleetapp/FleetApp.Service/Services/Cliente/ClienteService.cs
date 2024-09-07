using Dashdine.Domain.Interface.Cliente;
using Dashdine.Service.Exceptions.Cliente;
using Dashdine.Service.Extensions;
using Dashdine.Service.Interface.Cliente;
using Dashdine.Service.Interface.Usuario;
using Dashdine.Service.Models.Cliente;
using Microsoft.EntityFrameworkCore;

namespace Dashdine.Service.Services.Cliente;

public class ClienteService(IClienteRepository clienteRepository, ITokenService tokenService) : IClienteService
{
    public async Task<ProjecaoDeClienteLogado> Novo(DtoDeCliente clienteNovo)
    {
        var cliente = new Domain.Domain.Cliente.ClienteDomain(
            clienteNovo.Nome, clienteNovo.Sobrenome, clienteNovo.CPF,
            clienteNovo.DataNascimento, clienteNovo.Email, clienteNovo.Celular, clienteNovo.Senha);

        var informacaoDuplicada = await clienteRepository.ObterInformacaoUnicaJaCadastrada(cliente);
        if (!string.IsNullOrEmpty(informacaoDuplicada)) throw new InformacaoDeClienteDuplicadaException(informacaoDuplicada);

        await clienteRepository.Novo(cliente);
        return new ProjecaoDeClienteLogado()
        {
            Token = tokenService.GerarToken(cliente.Id, cliente.Nome, cliente.Email, cliente.Celular, CrossCutting.Enums.Usuario.EnumTipoDeUsuario.Cliente, cliente.Situacao.Id),
            Cliente = cliente.AsProjecao()
        };
    }

    public async Task Editar(Guid idUsuario, DtoDeCliente dtoCliente)
    {
        var usuario = await clienteRepository.UnitOfWork.Clientes.FirstOrDefaultAsync(u => u.Id == idUsuario)
            ?? throw new ClienteNaoEncontradoException();

        usuario.Nome = string.IsNullOrEmpty(dtoCliente.Nome) ? usuario.Nome : dtoCliente.Nome;
        usuario.Sobrenome = string.IsNullOrEmpty(dtoCliente.Sobrenome) ? usuario.Sobrenome : dtoCliente.Sobrenome;
        await clienteRepository.SaveChangesAsync();
    }
}
