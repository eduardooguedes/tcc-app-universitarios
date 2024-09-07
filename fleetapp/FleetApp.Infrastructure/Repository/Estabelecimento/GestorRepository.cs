using Dashdine.CrossCutting.Enums;
using Dashdine.Domain.Domain.Estabelecimento;
using Dashdine.Domain.Domain.Estabelecimento.Gestor;
using Dashdine.Domain.Interface.Estabelecimento;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;

namespace Dashdine.Infrastructure.Repository.Estabelecimento;

public class GestorRepository(IConfiguration configuration) : BaseRepository<Domain.Entitys.Gestor>(configuration), IGestorRepository
{
    public async Task<string> ObterInformacaoUnicaJaCadastrada(Gestor gestor)
    {
        var gestorCadastrado = await UnitOfWork.Gestors.Where(g => g.Cpf.Equals(gestor.CPF) || g.Email.Equals(gestor.Email)).FirstOrDefaultAsync();
        if (gestorCadastrado is null)
            return string.Empty;

        if (gestorCadastrado.Email.Equals(gestor.Email))
            return gestor.Email;

        return gestor.CPF;
    }

    public async Task Novo(Gestor gestor)
    {
        await UnitOfWork.Gestors.AddAsync(
            new Domain.Entitys.Gestor()
            {
                Id = gestor.Id,
                IdSituacao = gestor.Situacao.Id,
                Nome = gestor.Nome,
                Sobrenome = gestor.Sobrenome,
                Cpf = gestor.CPF,
                Senha = gestor.SenhaCriptografada,
                Email = gestor.Email,
                DataNascimento = gestor.DataDeNascimento,
                DataHoraCadastro = gestor.DataHoraCadastro,
                EmailConfirmado = gestor.EmailConfirmado,
            });
        await SaveChangesAsync();
    }

    public async Task<Gestor?> ObterPorEmail(string email)
    {
        var entidade = await UnitOfWork.Gestors
                        .Include(e => e.IdEstabelecimentoNavigation).ThenInclude(er => er!.EnderecoEstabelecimentos)
                        .Include(e => e.IdEstabelecimentoNavigation).ThenInclude(e => e.IdSituacaoNavigation)
                        .Where(g => g.Email == email)
                        .FirstOrDefaultAsync();

        if (entidade is null)
            return null;

        Domain.Domain.Estabelecimento.EstabelecimentoDomain? estabelecimento = null;
        if (entidade.IdEstabelecimentoNavigation != null)
        {
            EnderecoRetirada? enderecoRetirada = null;
            if (entidade.IdEstabelecimentoNavigation?.EnderecoEstabelecimentos.Count > 0)
            {
                var enderecoEntidade = entidade.IdEstabelecimentoNavigation.EnderecoEstabelecimentos.First();
                enderecoRetirada = new(enderecoEntidade.Id, enderecoEntidade.Cep, enderecoEntidade.Bairro, enderecoEntidade.Estado,
                    enderecoEntidade.Cidade, enderecoEntidade.Logradouro, enderecoEntidade.Numero, enderecoEntidade.Complemento, enderecoEntidade.Latitude, enderecoEntidade.Longitude, enderecoEntidade.Timezone);
            }

            estabelecimento = new Domain.Domain.Estabelecimento.EstabelecimentoDomain(entidade.IdEstabelecimentoNavigation!.Id, new(entidade.IdEstabelecimentoNavigation!.IdSituacaoNavigation.Id, entidade.IdEstabelecimentoNavigation!.IdSituacaoNavigation.Descricao), entidade.IdEstabelecimentoNavigation!.DataHoraCadastro, entidade.IdEstabelecimentoNavigation!.NomeFantasia, entidade.IdEstabelecimentoNavigation.RazaoSocial, entidade.IdEstabelecimentoNavigation.Cnpj, entidade.IdEstabelecimentoNavigation.Telefone, entidade.IdEstabelecimentoNavigation.Logo, enderecoRetirada);
        }

        return new Gestor(entidade.Id, estabelecimento, SituacaoDeGestor.ObterPorIdOuNovo(entidade.IdSituacao), entidade.DataHoraCadastro, entidade.Nome, entidade.Sobrenome, entidade.Cpf, entidade.DataNascimento, entidade.Email, entidade.EmailConfirmado, entidade.Senha);
    }

    public async Task<Gestor?> ObterPorIdComEstabelecimento(Guid id)
    {
        var entidade = await UnitOfWork.Gestors
                        .Include(e => e.IdEstabelecimentoNavigation).ThenInclude(er => er!.EnderecoEstabelecimentos)
                        .Include(e => e.IdEstabelecimentoNavigation).ThenInclude(e => e.IdSituacaoNavigation)
                        .Where(g => g.Id == id)
                        .FirstOrDefaultAsync();

        if (entidade?.IdEstabelecimentoNavigation is null)
            return null;

        var enderecoEntidade = entidade.IdEstabelecimentoNavigation.EnderecoEstabelecimentos.First();
        EnderecoRetirada enderecoRetirada = new(enderecoEntidade.Id, enderecoEntidade.Cep, enderecoEntidade.Bairro, enderecoEntidade.Estado,
            enderecoEntidade.Cidade, enderecoEntidade.Logradouro, enderecoEntidade.Numero, enderecoEntidade.Complemento, enderecoEntidade.Latitude, enderecoEntidade.Longitude, enderecoEntidade.Timezone);

        var estabelecimento = new EstabelecimentoDomain(
            entidade.IdEstabelecimentoNavigation.Id,
            new(entidade.IdEstabelecimentoNavigation.IdSituacao, entidade.IdEstabelecimentoNavigation.IdSituacaoNavigation.Descricao),
            entidade.IdEstabelecimentoNavigation.DataHoraCadastro,
            entidade.IdEstabelecimentoNavigation.NomeFantasia,
            entidade.IdEstabelecimentoNavigation.RazaoSocial,
            entidade.IdEstabelecimentoNavigation.Cnpj,
            entidade.IdEstabelecimentoNavigation.Telefone,
            entidade.IdEstabelecimentoNavigation.Logo,
            enderecoRetirada);

        return new(entidade.Id, estabelecimento, SituacaoDeGestor.ObterPorIdOuNovo(entidade.IdSituacao), entidade.DataHoraCadastro, entidade.Nome, entidade.Sobrenome, entidade.Cpf, entidade.DataNascimento, entidade.Email, entidade.EmailConfirmado, entidade.Senha);
    }

    public async Task<Gestor?> ObterPorId(Guid id)
    {
        var entidade = await UnitOfWork.Gestors
                        .Include(e => e.IdEstabelecimentoNavigation).ThenInclude(er => er!.EnderecoEstabelecimentos!)
                        .Include(e => e.IdEstabelecimentoNavigation).ThenInclude(e => e!.IdSituacaoNavigation)
                        .Where(g => g.Id == id)
                        .FirstOrDefaultAsync();

        if (entidade is null)
            return null;

        Domain.Domain.Estabelecimento.EstabelecimentoDomain? estabelecimento = null;
        if (entidade.IdEstabelecimentoNavigation != null)
        {
            var enderecoEntidade = entidade.IdEstabelecimentoNavigation.EnderecoEstabelecimentos.First();
            EnderecoRetirada enderecoRetirada = new(enderecoEntidade.Id, enderecoEntidade.Cep, enderecoEntidade.Bairro, enderecoEntidade.Estado,
                enderecoEntidade.Cidade, enderecoEntidade.Logradouro, enderecoEntidade.Numero, enderecoEntidade.Complemento, enderecoEntidade.Latitude, enderecoEntidade.Longitude, enderecoEntidade.Timezone);

            estabelecimento = new Domain.Domain.Estabelecimento.EstabelecimentoDomain(
                entidade.IdEstabelecimentoNavigation.Id, new(entidade.IdEstabelecimentoNavigation.IdSituacao, entidade.IdEstabelecimentoNavigation.IdSituacaoNavigation.Descricao),
                entidade.IdEstabelecimentoNavigation.DataHoraCadastro, entidade.IdEstabelecimentoNavigation.NomeFantasia, entidade.IdEstabelecimentoNavigation.RazaoSocial, entidade.IdEstabelecimentoNavigation.Cnpj, entidade.IdEstabelecimentoNavigation.Telefone, entidade.IdEstabelecimentoNavigation.Logo, enderecoRetirada);
        }

        return new(entidade.Id, estabelecimento, SituacaoDeGestor.ObterPorIdOuNovo(entidade.IdSituacao), entidade.DataHoraCadastro, entidade.Nome, entidade.Sobrenome, entidade.Cpf, entidade.DataNascimento, entidade.Email, entidade.EmailConfirmado, entidade.Senha);
    }

    public async Task<List<EstabelecimentosDoGestor>?> ObterListaDeEstabelecimentosDoGestor(Guid idGestor)
    {
        var gestor = await UnitOfWork.Gestors
                       .Where(g => g.Id == idGestor)
                       .Include(g => g.IdEstabelecimentoNavigation)
                       .FirstOrDefaultAsync();

        if (gestor is null)
            return null;

        List<EstabelecimentosDoGestor> estabelecimentos = [];
        if (gestor.IdEstabelecimentoNavigation is null)
            return estabelecimentos;

        estabelecimentos.Add(new EstabelecimentosDoGestor()
        {
            Id = gestor.IdEstabelecimentoNavigation.Id,
            Situacao = SituacaoDeEstabelecimento.ObterPorIdOuInativo(gestor.IdEstabelecimentoNavigation.IdSituacao)
        });

        return estabelecimentos;
    }
}
