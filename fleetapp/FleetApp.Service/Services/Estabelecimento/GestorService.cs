using Dashdine.Domain.Domain.Estabelecimento.Gestor;
using Dashdine.Domain.Interface.Estabelecimento;
using Dashdine.Service.Exceptions;
using Dashdine.Service.Exceptions.Gestor;
using Dashdine.Service.Interface.Estabelecimento;
using Dashdine.Service.Interface.Usuario;
using Dashdine.Service.Models.Estabelecimento.Gestor;
using Microsoft.EntityFrameworkCore;

namespace Dashdine.Service.Services.Estabelecimento;

public class GestorService(
    IGestorRepository gestorRepository,
    ITokenService tokenService) : IGestorService
{
    public async Task<ProjecaoDeGestor> Obter(Guid idGestor)
    {
        var gestorCadastrado = await gestorRepository.ObterPorId(idGestor)
            ?? throw new GestorNaoEncontradoException();

        return new ProjecaoDeGestor()
        {
            Id = gestorCadastrado.Id.ToString(),
            IdSituacao = gestorCadastrado.Situacao.Id,
            Nome = gestorCadastrado.Nome,
            Sobrenome = gestorCadastrado.Sobrenome,
            CPF = gestorCadastrado.CPF,
            DataDeNascimento = gestorCadastrado.DataDeNascimento,
            DataDeCadastro = gestorCadastrado.DataHoraCadastro,
            Email = gestorCadastrado.Email,
            EmailConfirmado = gestorCadastrado.EmailConfirmado,
        };
    }

    public async Task<ProjecaoDeGestorLogado> Novo(DtoDeGestor dto)
    {
        var gestor = new Gestor(dto.Nome, dto.Sobrenome, dto.CPF, dto.DataNascimento, dto.Email, dto.Senha);

        var informacaoDuplicada = await gestorRepository.ObterInformacaoUnicaJaCadastrada(gestor);
        if (!string.IsNullOrEmpty(informacaoDuplicada))
            throw new ServiceException($"{informacaoDuplicada} já vinculado a um gestor.");

        await gestorRepository.Novo(gestor);
        return new ProjecaoDeGestorLogado()
        {
            Token = tokenService.GerarToken(gestor.Id, gestor.Nome, gestor.Email, string.Empty, CrossCutting.Enums.Usuario.EnumTipoDeUsuario.Gestor, gestor.Situacao.Id),
            Gestor = new ProjecaoDeGestor()
            {
                Id = gestor.Id.ToString(),
                IdSituacao = gestor.Situacao.Id,
                Nome = gestor.Nome,
                Sobrenome = gestor.Sobrenome,
                CPF = gestor.CPF,
                DataDeCadastro = gestor.DataHoraCadastro,
                DataDeNascimento = gestor.DataDeNascimento,
                Email = gestor.Email,
                EmailConfirmado = gestor.EmailConfirmado,
            }
        };
    }

    public async Task Editar(Guid idUsuario, DtoDeEdicaoDeGestor dtoGestor)
    {
        var usuario = await gestorRepository.UnitOfWork.Gestors.FirstOrDefaultAsync(u => u.Id == idUsuario) ?? throw new GestorNaoEncontradoException();
        usuario.Nome = string.IsNullOrEmpty(dtoGestor.Nome) ? usuario.Nome : dtoGestor.Nome;
        usuario.Sobrenome = string.IsNullOrEmpty(dtoGestor.Sobrenome) ? usuario.Sobrenome : dtoGestor.Sobrenome;
        usuario.DataNascimento = dtoGestor.DataNascimento ?? usuario.DataNascimento;
        await gestorRepository.SaveChangesAsync();
    }
}
