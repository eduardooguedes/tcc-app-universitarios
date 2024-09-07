using Dashdine.CrossCutting.Enums;
using Dashdine.CrossCutting.Enums.Usuario;
using Dashdine.Domain.Context;
using Dashdine.Domain.Domain.Estabelecimento;
using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Domain.Interface;
using Dashdine.Domain.Interface.Estabelecimento;
using Dashdine.Domain.Interface.Geolocalizacao;
using Dashdine.Service.Exceptions;
using Dashdine.Service.Exceptions.Estabelecimento;
using Dashdine.Service.Exceptions.Gestor;
using Dashdine.Service.Exceptions.Usuario;
using Dashdine.Service.Interface.Estabelecimento;
using Dashdine.Service.Models.Estabelecimento;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dashdine.Service.Services.Estabelecimento;

public class EstabelecimentoService(
    IEstabelecimentoRepository estabelecimentoRepository,
    IGestorRepository gestorRepository,
    ILogoDoEstabelecimentoRepository logoDoEstabelecimentoRepository,
    IGeolocalizacaoRepository geolocalizacaoRepository,
    ITimeZoneRepository timeZoneRepository,
    IParametroRepository parametroRepository) : IEstabelecimentoService
{
    public async Task<Guid> Cadastrar(UsuarioAutenticado gestorAutenticado, DtoDeEstabelecimento dto)
    {
        var gestor = await gestorRepository.ObterPorId(gestorAutenticado.Id) ?? throw new GestorNaoEncontradoException();

        if (gestor?.Estabelecimento != null)
            throw new GestorJaVinculadoAOutroEstabelecimentoException();

        var geolocalizacao = await geolocalizacaoRepository.Obter(dto.EnderecoRetirada.Logradouro, dto.EnderecoRetirada.Numero, dto.EnderecoRetirada.Complemento, dto.EnderecoRetirada.Cidade, dto.EnderecoRetirada.Estado) ?? throw new EnderecoInformadoNaoEncontradoException();

        EnderecoRetirada endereco = new(Guid.NewGuid(), dto.EnderecoRetirada.CEP, dto.EnderecoRetirada.Bairro, dto.EnderecoRetirada.Estado,
            dto.EnderecoRetirada.Cidade, dto.EnderecoRetirada.Logradouro, dto.EnderecoRetirada.Numero, dto.EnderecoRetirada.Complemento, geolocalizacao.Latitude!.Value, geolocalizacao.Longitude!.Value, geolocalizacao.Timezone!);


        var estabelecimento = new EstabelecimentoDomain(Guid.NewGuid(), SituacaoDeEstabelecimento.Novo, (await timeZoneRepository.Obter(endereco.TimeZone)).DataHoraAtual, dto.NomeFantasia, dto.RazaoSocial, dto.CNPJ, dto.Telefone, await parametroRepository.ObterLogoPadraoEstabelecimento(), endereco);

        var informacaoDuplicada = await estabelecimentoRepository.ObterInformacaoUnicaJaCadastrada(estabelecimento);
        if (!string.IsNullOrEmpty(informacaoDuplicada))
            throw new ServiceException($"{informacaoDuplicada} já vinculado a um estabelecimento.");

        await estabelecimentoRepository.Cadastrar(gestor!.Id, estabelecimento);
        return estabelecimento.Id;
    }

    public async Task Editar(UsuarioAutenticado gestorAutenticado, Guid idEstabelecimento, DtoDeEstabelecimentoEdicao dto)
    {
        if (gestorAutenticado.TipoDeUsuario != EnumTipoDeUsuario.Gestor)
            throw new UsuarioNaoEncontradoException();

        var gestor = await gestorRepository.UnitOfWork.Gestors
            .Include(g => g.IdEstabelecimentoNavigation)
            .Where(g => g.Id == gestorAutenticado.Id)
            .FirstAsync();

        if (gestor.IdEstabelecimentoNavigation == null || gestor.IdEstabelecimentoNavigation.Id != idEstabelecimento)
            throw new EstabelecimentoNaoEncontradoException();

        gestor.IdEstabelecimentoNavigation.NomeFantasia = dto.NomeFantasia ?? gestor.IdEstabelecimentoNavigation.NomeFantasia;
        gestor.IdEstabelecimentoNavigation.RazaoSocial = dto.RazaoSocial ?? gestor.IdEstabelecimentoNavigation.RazaoSocial;
        gestor.IdEstabelecimentoNavigation.Telefone = dto.Telefone ?? gestor.IdEstabelecimentoNavigation.Telefone;
        await gestorRepository.SaveChangesAsync();
    }

    public async Task EditarEnderecoRetirada(UsuarioAutenticado gestorAutenticado, Guid idEstabelecimento, Guid idEndereco, DtoDeEnderecoRetirada dtoEndereco)
    {
        if (gestorAutenticado.TipoDeUsuario != EnumTipoDeUsuario.Gestor)
            throw new UsuarioNaoEncontradoException();

        var gestor = await gestorRepository.UnitOfWork.Gestors
            .Include(g => g.IdEstabelecimentoNavigation).ThenInclude(e => e!.EnderecoEstabelecimentos)
            .Where(g => g.Id == gestorAutenticado.Id)
            .FirstAsync();

        if (gestor.IdEstabelecimentoNavigation == null || gestor.IdEstabelecimentoNavigation.Id != idEstabelecimento)
            throw new EstabelecimentoNaoEncontradoException();

        var endereco = gestor.IdEstabelecimentoNavigation.EnderecoEstabelecimentos.FirstOrDefault(e => e.Id.Equals(idEndereco))
            ?? throw new EnderecoDoEstabelecimentoNaoEncontradoException();

        var geolocalizacao = await geolocalizacaoRepository.Obter(dtoEndereco.Logradouro, dtoEndereco.Numero, dtoEndereco.Complemento, dtoEndereco.Cidade, dtoEndereco.Estado) ?? throw new EnderecoInformadoNaoEncontradoException();

        endereco.Logradouro = dtoEndereco.Logradouro;
        endereco.Numero = dtoEndereco.Numero;
        endereco.Bairro = dtoEndereco.Bairro;
        endereco.Cep = dtoEndereco.CEP;
        endereco.Bairro = dtoEndereco.Bairro;
        endereco.Cidade = dtoEndereco.Cidade;
        endereco.Estado = dtoEndereco.Estado;
        endereco.Complemento = dtoEndereco.Complemento;
        endereco.Latitude = geolocalizacao.Latitude!.Value;
        endereco.Longitude = geolocalizacao.Longitude!.Value;
        endereco.Timezone = geolocalizacao.Timezone!;

        await gestorRepository.SaveChangesAsync();
    }

    public async Task<ProjecaoDeEstabelecimento> Obter(UsuarioAutenticado gestorAutenticado, Guid idEstabelecimento)
    {
        if (gestorAutenticado.TipoDeUsuario != EnumTipoDeUsuario.Gestor)
            throw new UsuarioNaoEncontradoException();

        var gestor = await gestorRepository.UnitOfWork.Gestors
            .Include(g => g.IdEstabelecimentoNavigation).ThenInclude(e => e!.EnderecoEstabelecimentos)
            .Include(g => g.IdEstabelecimentoNavigation).ThenInclude(e => e!.IdSituacaoNavigation)
            .Where(g => g.Id == gestorAutenticado.Id)
            .FirstAsync();

        if (gestor.IdEstabelecimentoNavigation == null || gestor.IdEstabelecimentoNavigation.Id != idEstabelecimento)
            throw new EstabelecimentoNaoEncontradoException();

        var estabelecimento = gestor.IdEstabelecimentoNavigation;
        var enderecoRetirada = estabelecimento.EnderecoEstabelecimentos.FirstOrDefault();
        return new ProjecaoDeEstabelecimento()
        {
            Id = estabelecimento.Id.ToString(),
            Situacao = new SituacaoDeEstabelecimento(estabelecimento.IdSituacao, estabelecimento.IdSituacaoNavigation.Descricao),
            NomeFantasia = estabelecimento.NomeFantasia,
            RazaoSocial = estabelecimento.RazaoSocial,
            Logo = estabelecimento.Logo,
            DataHoraCadastro = estabelecimento.DataHoraCadastro,
            Telefone = estabelecimento.Telefone,
            EnderecoRetirada = enderecoRetirada != null ? new ProjecaoDeEnderecoEstabelecimento()
            {
                Id = enderecoRetirada.Id.ToString(),
                CEP = enderecoRetirada.Cep,
                Bairro = enderecoRetirada.Bairro,
                Logradouro = enderecoRetirada.Logradouro,
                Numero = enderecoRetirada.Numero,
                Complemento = enderecoRetirada.Complemento,
                Cidade = enderecoRetirada.Cidade,
                Estado = enderecoRetirada.Estado,
                Latitude = enderecoRetirada.Latitude,
                Longitude = enderecoRetirada.Longitude
            } : null,
        };
    }

    public async Task<ProjecaoDeInformacoesDoEstabelecimento> ObterInformacoes(UsuarioAutenticado gestorAutenticado, Guid idEstabelecimento)
    {
        if (gestorAutenticado.TipoDeUsuario != EnumTipoDeUsuario.Gestor)
            throw new UsuarioNaoEncontradoException();

        var estabelecimentosDoGestor = await gestorRepository.ObterListaDeEstabelecimentosDoGestor(gestorAutenticado.Id) ?? throw new GestorNaoEncontradoException();

        if (!estabelecimentosDoGestor.Any(e => e.Id.Equals(idEstabelecimento)))
            throw new EstabelecimentoNaoEncontradoException();

        var informacoes = await estabelecimentoRepository.ObterInformacoes(idEstabelecimento, DateTime.Now) ?? throw new EstabelecimentoNaoEncontradoException();
        return new ProjecaoDeInformacoesDoEstabelecimento()
        {
            Id = informacoes.Id.ToString(),
            NomeFantasia = informacoes.NomeFantasia,
            EnderecoCompleto = informacoes.EnderecoCompleto,
            InformacoesSobreOsProdutos = new Domain.Domain.Produto.InformacoesSobreOsProdutos() { QuantidadeDeProdutosCadastrados = informacoes.InformacoesSobreOsProdutos.QuantidadeDeProdutosCadastrados },
        };
    }

    public async Task<string> AtualizarLogo(UsuarioAutenticado gestorAutenticado, Guid idEstabelecimento, IFormFile logo)
    {
        if (gestorAutenticado.TipoDeUsuario != EnumTipoDeUsuario.Gestor)
            throw new UsuarioNaoEncontradoException();

        var gestor = await gestorRepository.ObterPorIdComEstabelecimento(gestorAutenticado.Id) ?? throw new GestorNaoPossuiEstabelecimentoException();

        if (gestor!.Estabelecimento!.Id != idEstabelecimento)
            throw new EstabelecimentoNaoEncontradoException();

        var estabelecimento = await estabelecimentoRepository.UnitOfWork.Estabelecimentos.FirstAsync(e => e.Id == idEstabelecimento);
        try
        {
            estabelecimento.Logo = await logoDoEstabelecimentoRepository.AtualizarLogo(idEstabelecimento, logo);
        }
        catch (Exception)
        {
            throw new ServiceException("Não foi possível atualizar a logo. Tente novamente mais tarde.");
        }
        await estabelecimentoRepository.SaveChangesAsync();
        return estabelecimento.Logo;
    }

    public async Task AtualizarSituacao(UsuarioAutenticado usuarioAutenticado, Guid idEstabelecimento, int novaSituacao)
    {
        var gestor = await gestorRepository.ObterPorId(usuarioAutenticado.Id) ?? throw new GestorNaoPossuiEstabelecimentoException();

        var estabelecimento = await estabelecimentoRepository.UnitOfWork.Estabelecimentos.FirstOrDefaultAsync(e => e.Id.Equals(idEstabelecimento)) ?? throw new EstabelecimentoNaoEncontradoException();

        var situacao = await estabelecimentoRepository.UnitOfWork.SituacaoEstabelecimentos.FirstOrDefaultAsync(s => s.Id.Equals(novaSituacao)) ?? throw new SituacaoDeEstabelecimentoNaoEncontradaException();

        if (situacao.Novo) throw new NaoEhPossivelAtualizarSituacaoException();

        estabelecimento.IdSituacao = situacao.Id;
        await estabelecimentoRepository.SaveChangesAsync();
    }

    public async Task RemoverLogo(UsuarioAutenticado gestorAutenticado, Guid idEstabelecimento)
    {
        if (gestorAutenticado.TipoDeUsuario != EnumTipoDeUsuario.Gestor)
            throw new UsuarioNaoEncontradoException();

        var gestor = await gestorRepository.ObterPorId(gestorAutenticado.Id) ?? throw new GestorNaoEncontradoException();

        if (gestor!.Estabelecimento == null || gestor.Estabelecimento.Id != idEstabelecimento)
            throw new EstabelecimentoNaoEncontradoException();

        try
        {
            await logoDoEstabelecimentoRepository.RemoverLogo(idEstabelecimento);
        }
        catch (Exception)
        {
            throw new ServiceException("Não foi possível remover a logo. Tente novamente mais tarde.");
        }

        var estabelecimento = await estabelecimentoRepository.UnitOfWork.Estabelecimentos.Where(e => e.Id.Equals(gestor.Estabelecimento.Id)).FirstAsync();
        estabelecimento.Logo = await parametroRepository.ObterLogoPadraoEstabelecimento();
        await estabelecimentoRepository.SaveChangesAsync();
    }
}
