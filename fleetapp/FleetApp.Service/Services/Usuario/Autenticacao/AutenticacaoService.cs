using Dashdine.CrossCutting.Enums.Usuario;
using Dashdine.Domain.Domain.Cliente;
using Dashdine.Domain.Domain.Estabelecimento.Gestor;
using Dashdine.Domain.Interface.Cliente;
using Dashdine.Domain.Interface.Estabelecimento;
using Dashdine.Service.Exceptions;
using Dashdine.Service.Exceptions.Usuario;
using Dashdine.Service.Interface.Usuario;
using Dashdine.Service.Models.Cliente;
using Dashdine.Service.Models.Estabelecimento;
using Dashdine.Service.Models.Estabelecimento.Gestor;
using Dashdine.Service.Models.Usuario;
using Microsoft.EntityFrameworkCore;

namespace Dashdine.Service.Services.Usuario.Autenticacao;

public class AutenticacaoService(
    IClienteRepository clienteRepository,
    IGestorRepository gestorRepository,
    ITokenService tokenService,
    IConfirmacaoDeIdentidadeService confirmacaoDeIdentidadeService) : IAutenticacaoService
{
    public async Task<ProjecaoDeClienteLogado?> LoginCliente(DtoDeLogin login)
    {
        ClienteDomain? cliente = await clienteRepository.ObterPorEmail(login.Email);

        ProjecaoDeClienteLogado? projecaoCliente = null;
        if (cliente?.SenhaEhValida(login.Senha) == true)
        {
            projecaoCliente = new ProjecaoDeClienteLogado()
            {
                Token = tokenService.GerarToken(cliente.Id, cliente.Nome, cliente.Email, cliente.Celular, EnumTipoDeUsuario.Cliente, cliente.Situacao.Id),
                Cliente = new ProjecaoDeCliente(
                    cliente.Id.ToString(),
                    cliente.Situacao.Id,
                    cliente.CPF,
                    cliente.Nome,
                    cliente.Sobrenome,
                    cliente.Email,
                    cliente.EmailConfirmado,
                    cliente.Celular,
                    cliente.CelularConfirmado,
                    cliente.DataHoraCadastro,
                    cliente.DataDeNascimento,
                    await clienteRepository.UnitOfWork.EnderecoClientes
                    .Where(e => e.IdCliente == cliente.Id)
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
                    })
                    .ToListAsync())
            };
        }
        return projecaoCliente;
    }

    public async Task<ProjecaoDeGestorLogado?> LoginGestor(DtoDeLogin login)
    {
        var gestor = await gestorRepository.ObterPorEmail(login.Email);
        if (gestor is null)
            return null;

        if (gestor?.SenhaEhValida(login.Senha) == true)
        {
            var estabelecimentos =
                (await gestorRepository.ObterListaDeEstabelecimentosDoGestor(gestor.Id))?
                 .Select(e => new ProjecaoParaListaDeEstabelecimentosDoGestor()
                 {
                     Id = e.Id.ToString(),
                     Situacao = e.Situacao
                 }).ToList();

            return new ProjecaoDeGestorLogado()
            {
                Token = tokenService.GerarToken(gestor.Id, gestor.Nome, gestor.Email, gestor.SenhaCriptografada, EnumTipoDeUsuario.Gestor, gestor.Situacao.Id),
                Estabelecimentos = estabelecimentos ?? [],
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
                },
            };
        }

        return null;
    }

    public async Task<double> SolicitarParaRecuperarSenha(string email, EnumTipoDeUsuario tipoDeUsuario)
    {
        string nomeUsuario = "", idUsuario = "", emailUsuario = "";

        if (tipoDeUsuario == EnumTipoDeUsuario.Cliente)
        {
            var cliente = await clienteRepository.UnitOfWork.Clientes.AsNoTracking().Where(u => u.Email == email).FirstOrDefaultAsync() ?? throw new ServiceException($"Cliente de e-mail '{email}' não encontrado.");

            nomeUsuario = cliente.Nome;
            idUsuario = cliente.Id.ToString();
            emailUsuario = cliente.Email;
            tipoDeUsuario = EnumTipoDeUsuario.Cliente;
        }
        else if (tipoDeUsuario == EnumTipoDeUsuario.Gestor)
        {
            var gestor = await clienteRepository.UnitOfWork.Gestors.AsNoTracking().Where(u => u.Email == email).FirstOrDefaultAsync() ?? throw new ServiceException($"Gestor de e-mail '{email}' não encontrado.");

            nomeUsuario = gestor.Nome;
            idUsuario = gestor.Id.ToString();
            emailUsuario = gestor.Email;
            tipoDeUsuario = EnumTipoDeUsuario.Gestor;
        }
        else
        {
            throw new UsuarioNaoPermitidoARecuperarSenhaException();
        }

        return await confirmacaoDeIdentidadeService.EnviarConfirmacaoParaRecuperarSenha(idUsuario, nomeUsuario, emailUsuario, tipoDeUsuario, CrossCutting.CrossCutting.Enums.Usuario.EnumCanalDeContato.Email);
    }

    public async Task<ProjecaoDeUsuarioLogado?> AutorizarRecuperacaoDeSenha(string email, string codigo, EnumTipoDeUsuario tipoDeUsuario)
    {
        if (tipoDeUsuario == EnumTipoDeUsuario.Cliente)
        {
            var cliente = await clienteRepository.UnitOfWork.Clientes.Where(u => u.Email == email).FirstOrDefaultAsync() ?? throw new ServiceException($"Cliente de e-mail '{email}' não encontrado.");

            if (confirmacaoDeIdentidadeService.ConfirmarIdentidadeEAutorizarAlteracaoSenha(cliente.Id, codigo, EnumTipoDeUsuario.Cliente))
            {
                if (!cliente.EmailConfirmado || cliente.IdSituacao == SituacaoDeCliente.Novo.Id)
                {
                    cliente.EmailConfirmado = true;
                    cliente.IdSituacao = SituacaoDeCliente.Ativo.Id;
                    await clienteRepository.SaveChangesAsync();
                }
                var user = new ClienteDomain(cliente.Id, SituacaoDeCliente.ObterPorIdOuNovo(cliente.IdSituacao), cliente.DataHoraCadastro,
                cliente.Nome, cliente.Sobrenome, cliente.Cpf, cliente.DataNascimento, cliente.Email, cliente.EmailConfirmado, cliente.Celular, cliente.CelularConfirmado, cliente.Senha);
                return new ProjecaoDeUsuarioLogado()
                {
                    Token = tokenService.GerarToken(user.Id, user.Nome, user.Email, user.Celular, EnumTipoDeUsuario.Cliente, user.Situacao.Id, true),
                    Usuario = new ProjecaoDeUsuario(user),
                };
            }
        }
        else if (tipoDeUsuario == EnumTipoDeUsuario.Gestor)
        {
            var entidade = await clienteRepository.UnitOfWork.Gestors
                .Include(g => g.IdSituacaoNavigation)
                .FirstOrDefaultAsync(u => u.Email == email) ?? throw new ServiceException($"Gestor de e-mail '{email}' não encontrado.");

            if (confirmacaoDeIdentidadeService.ConfirmarIdentidadeEAutorizarAlteracaoSenha(entidade.Id, codigo, EnumTipoDeUsuario.Gestor))
            {
                if (!entidade.EmailConfirmado || entidade.IdSituacao == SituacaoDeCliente.Novo.Id)
                {
                    entidade.EmailConfirmado = true;
                    entidade.IdSituacao = SituacaoDeCliente.Ativo.Id;
                    await clienteRepository.SaveChangesAsync();
                }
                var user = new Gestor(entidade.Id, new SituacaoDeGestor(entidade.IdSituacao, entidade.IdSituacaoNavigation.Descricao), entidade.DataHoraCadastro,
                entidade.Nome, entidade.Sobrenome, entidade.Cpf, entidade.DataNascimento, entidade.Email, entidade.EmailConfirmado, entidade.Senha);
                return new ProjecaoDeUsuarioLogado()
                {
                    Token = tokenService.GerarToken(user.Id, user.Nome, user.Email, string.Empty, EnumTipoDeUsuario.Gestor, user.Situacao.Id, true),
                    Usuario = new ProjecaoDeUsuario(user),
                };
            }
        }
        else
        {
            throw new UsuarioNaoPermitidoARecuperarSenhaException();
        }

        return null;
    }
}
