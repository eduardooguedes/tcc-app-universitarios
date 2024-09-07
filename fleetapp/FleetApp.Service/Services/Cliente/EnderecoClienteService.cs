using Dashdine.Domain.Domain.Cliente.EnderecoCliente;
using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Domain.Interface.Cliente;
using Dashdine.Domain.Interface.Geolocalizacao;
using Dashdine.Service.Exceptions;
using Dashdine.Service.Exceptions.Cliente;
using Dashdine.Service.Interface.Cliente;
using Dashdine.Service.Models.Cliente;
using Microsoft.EntityFrameworkCore;

namespace Dashdine.Service.Services.Cliente;

public class EnderecoClienteService(IEnderecoClienteRepository enderecoClienteRepository, IGeolocalizacaoRepository geolocalizacaoRepository, ITipoEnderecoClienteRepository tipoEnderecoClienteRepository) : IEnderecoClienteService
{
    private async Task ValidarSeExisteEnderecoComMesmoTipoEApelido(Guid idUsuario, string apelido, TipoEnderecoClienteDomain tipoEndereco)
    {
        if (await enderecoClienteRepository.UnitOfWork.EnderecoClientes
                .FirstOrDefaultAsync(e =>
                    e.IdCliente.Equals(idUsuario) &&
                    e.Apelido.Equals(apelido) &&
                    e.IdTipoEndereco.Equals(tipoEndereco.Id)) is not null)
        {
            throw new ServiceException($"Você já possui um endereço de {tipoEndereco.Descricao.ToLower()} com este apelido.");
        }
    }

    public async Task<Guid> Adicionar(UsuarioAutenticado usuarioAutenticado, DtoDeEnderecoCliente dto)
    {
        var tipoDeEndereco = await tipoEnderecoClienteRepository.Obter(dto.Tipo) ?? throw new TipoDeEnderecoNaoEncontradoException();

        await ValidarSeExisteEnderecoComMesmoTipoEApelido(usuarioAutenticado.Id, dto.Apelido, tipoDeEndereco);

        var geolocalizacao = await geolocalizacaoRepository.Obter(dto.Logradouro, dto.Numero, dto.Complemento, dto.Cidade, dto.Estado);
        if (geolocalizacao is null) throw new EnderecoInformadoNaoEncontradoException();

        var enderecoCliente = new EnderecoClienteDomain(Guid.NewGuid(), usuarioAutenticado.Id, tipoDeEndereco, dto.Apelido, dto.Principal, dto.CEP, dto.Logradouro, dto.Numero, dto.Complemento, dto.Bairro, dto.Cidade, dto.Estado, geolocalizacao.Latitude!.Value, geolocalizacao.Longitude!.Value, geolocalizacao.Timezone!);

        await enderecoClienteRepository.Adicionar(enderecoCliente);
        return enderecoCliente.Id;
    }

    public async Task Atualizar(UsuarioAutenticado usuarioAutenticado, Guid idEndereco, DtoDeEnderecoCliente dto)
    {
        var tipoDeEndereco = await tipoEnderecoClienteRepository.Obter(dto.Tipo) ?? throw new TipoDeEnderecoNaoEncontradoException();

        var endereco = await enderecoClienteRepository.Obter(idEndereco)
            ?? throw new EnderecoDoClienteNaoEncontradoException();

        if (!endereco.IdCliente.Equals(usuarioAutenticado.Id))
            throw new EnderecoDoClienteNaoEncontradoException();

        if (dto.Apelido != endereco.Apelido)
            await ValidarSeExisteEnderecoComMesmoTipoEApelido(usuarioAutenticado.Id, dto.Apelido, tipoDeEndereco);
        
        var geolocalizacao = await geolocalizacaoRepository.Obter(endereco.Logradouro, endereco.Numero, endereco.Complemento, endereco.Cidade, endereco.Estado);

        if (geolocalizacao is null) throw new EnderecoInformadoNaoEncontradoException();

        var enderecoAtualizado = endereco with
        {
            Apelido = dto.Apelido,
            Cep = dto.CEP,
            Estado = dto.Estado,
            Cidade = dto.Cidade,
            Bairro = dto.Bairro,
            Numero = dto.Numero,
            Complemento = dto.Complemento,
            Logradouro = dto.Logradouro,
            Principal = dto.Principal,
            Latitude = geolocalizacao.Latitude!.Value,
            Longitude = geolocalizacao.Longitude!.Value,
            Timezone = geolocalizacao.Timezone!
        };

        await enderecoClienteRepository.Atualizar(enderecoAtualizado);
    }

    public async Task<IEnumerable<ProjecaoDeEnderecoCliente>> ObterLista(UsuarioAutenticado usuarioAutenticado, int? idTipoEndereco)
    {
        TipoEnderecoClienteDomain? tipoEndereco = null;
        if (idTipoEndereco is not null)
            tipoEndereco = await tipoEnderecoClienteRepository.Obter(idTipoEndereco.Value) ?? throw new TipoDeEnderecoNaoEncontradoException();

        Domain.Entitys.Cliente? cliente =
            await
                (tipoEndereco != null ?
                enderecoClienteRepository.UnitOfWork
                .Clientes
                .Include(e => e.EnderecoClientes.Where(e => e.IdTipoEndereco == tipoEndereco.Id))
                .FirstOrDefaultAsync(u => u.Id == usuarioAutenticado.Id)
                :
                enderecoClienteRepository.UnitOfWork
                .Clientes
                .Include(e => e.EnderecoClientes)
                .FirstOrDefaultAsync(u => u.Id == usuarioAutenticado.Id));

        return cliente is null ? throw new ClienteNaoEncontradoException() :
            cliente.EnderecoClientes
            .Select(e => new ProjecaoDeEnderecoCliente()
            {
                Id = e.Id.ToString(),
                Apelido = e.Apelido,
                Bairro = e.Bairro,
                Cidade = e.Cidade,
                CEP = e.Cep,
                Estado = e.Estado,
                Complemento = e.Complemento,
                Logradouro = e.Logradouro,
                Numero = e.Numero,
                Principal = e.Principal,
                Tipo = e.IdTipoEndereco,
                Latitude = e.Latitude,
                Longitude = e.Longitude
            });
    }

    public async Task Remover(UsuarioAutenticado usuarioAutenticado, Guid idEndereco)
    {
        var enderecoCobranca = await enderecoClienteRepository.UnitOfWork.EnderecoClientes
            .Include(c => c.CartaoClientes)
            .Where(e => e.IdCliente == usuarioAutenticado.Id && e.Id == idEndereco)
            .FirstOrDefaultAsync()
            ?? throw new EnderecoDoClienteNaoEncontradoException();

        if (enderecoCobranca.CartaoClientes.Count > 0)
            throw new EnderecoDeCobrancaPossuiCartoesException();

        if (await enderecoClienteRepository.UnitOfWork.Pedidos.AnyAsync(p => p.IdLocalizacaoCliente.Equals(idEndereco)))
            throw new EnderecoDeRetiradaPossuiPedidosException();

        enderecoClienteRepository.UnitOfWork.EnderecoClientes.Remove(enderecoCobranca);
        await enderecoClienteRepository.SaveChangesAsync();
    }
}
