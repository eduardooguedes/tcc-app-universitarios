using Dashdine.CrossCutting.Enums.Usuario;
using Dashdine.Domain.Domain.Cliente;
using Dashdine.Domain.Domain.Estabelecimento.Gestor;
using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Domain.Interface.Cliente;
using Dashdine.Service.Exceptions;
using Dashdine.Service.Exceptions.Cliente;
using Dashdine.Service.Exceptions.Gestor;
using Dashdine.Service.Exceptions.Usuario;
using Dashdine.Service.Interface.Usuario;
using Dashdine.Service.Models.Usuario;
using Microsoft.EntityFrameworkCore;

namespace Dashdine.Service.Services.Usuario;

public class UsuarioService(
    IClienteRepository usuarioRepository,
    ITokenService tokenService,
    IConfirmacaoDeIdentidadeService confirmacaoDeIdentidadeService) : IUsuarioService
{
    public async Task<double> SolicitarParaAtualizarEmail(UsuarioAutenticado usuario, string novoEmail)
    {
        string idUsuario, nome;
        EnumTipoDeUsuario tipoDeUsuario;
        if (usuario.TipoDeUsuario == EnumTipoDeUsuario.Cliente)
        {
            var entidade = await usuarioRepository.UnitOfWork.Clientes.FirstOrDefaultAsync(u => u.Id.Equals(usuario.Id)) ?? throw new ClienteNaoEncontradoException();

            if (await usuarioRepository.UnitOfWork.Clientes.FirstOrDefaultAsync(u => u.Email == novoEmail && u.EmailConfirmado) != null)
                throw new ServiceException($"E-mail '{novoEmail}' já vinculado a outro cliente.");

            idUsuario = entidade.Id.ToString();
            nome = entidade.Nome;
            tipoDeUsuario = EnumTipoDeUsuario.Cliente;
        }
        else if (usuario.TipoDeUsuario == EnumTipoDeUsuario.Gestor)
        {
            var entidade = await usuarioRepository.UnitOfWork.Gestors.FirstOrDefaultAsync(u => u.Id.Equals(usuario.Id)) ?? throw new GestorNaoEncontradoException();

            if (await usuarioRepository.UnitOfWork.Gestors.FirstOrDefaultAsync(u => u.Email == novoEmail && u.EmailConfirmado) != null)
                throw new ServiceException($"E-mail '{novoEmail}' já vinculado a outro gestor.");

            idUsuario = entidade.Id.ToString();
            nome = entidade.Nome;
            tipoDeUsuario = EnumTipoDeUsuario.Gestor;
        }
        else
        {
            return 0;
        }

        return await confirmacaoDeIdentidadeService.EnviarConfirmacaoParaAtualizarEmail(usuario.Id.ToString(), usuario.Nome, novoEmail, tipoDeUsuario);
    }

    public async Task<bool> ConfirmarAtualizacaoDeEmail(UsuarioAutenticado usuarioAutenticado, string codigo)
    {
        string? novoEmail = confirmacaoDeIdentidadeService.ConfirmarCodigoEObterNovoEmail(usuarioAutenticado.Id, codigo, usuarioAutenticado.TipoDeUsuario);
        if (string.IsNullOrEmpty(novoEmail))
            return false;

        if (usuarioAutenticado.TipoDeUsuario == EnumTipoDeUsuario.Cliente)
        {
            if (await usuarioRepository.UnitOfWork.Clientes.Where(u => u.Email == novoEmail && u.EmailConfirmado).FirstOrDefaultAsync() != null)
                throw new ServiceException("E-mail escolhido está vinculado a outro cliente.");

            var usuario = await usuarioRepository.UnitOfWork.Clientes.FirstOrDefaultAsync(u => u.Id == usuarioAutenticado.Id) ?? throw new ClienteNaoEncontradoException();
            if (usuario.IdSituacao == SituacaoDeCliente.Novo.Id)
                usuario.IdSituacao = SituacaoDeCliente.Ativo.Id;
            usuario.Email = novoEmail;
            usuario.EmailConfirmado = true;
        }
        else if (usuarioAutenticado.TipoDeUsuario == EnumTipoDeUsuario.Gestor)
        {
            if (await usuarioRepository.UnitOfWork.Gestors.Where(u => u.Email == novoEmail && u.EmailConfirmado).FirstOrDefaultAsync() != null)
                throw new ServiceException("E-mail escolhido está vinculado a outro gestor.");

            var usuario = await usuarioRepository.UnitOfWork.Gestors.FirstOrDefaultAsync(u => u.Id == usuarioAutenticado.Id) ?? throw new GestorNaoEncontradoException();
            if (usuario.IdSituacao == SituacaoDeGestor.Novo.Id)
                usuario.IdSituacao = SituacaoDeGestor.Ativo.Id;
            usuario.Email = novoEmail;
            usuario.EmailConfirmado = true;
        }

        await usuarioRepository.SaveChangesAsync();
        return true;
    }

    public async Task<ProjecaoDeUsuarioLogado> RecuperarSenha(UsuarioAutenticado usuarioAutenticado, DtoDeRecuperacaoDeSenha dto)
    {
        if (!usuarioAutenticado.AutorizadoARecuperarSenha())
            throw new UsuarioNaoPermitidoARecuperarSenhaException();

        if (usuarioAutenticado.TipoDeUsuario == EnumTipoDeUsuario.Cliente)
        {
            var entidade = await usuarioRepository.UnitOfWork.Clientes.FirstOrDefaultAsync(u => u.Id == usuarioAutenticado.Id) ?? throw new ClienteNaoEncontradoException();

            var usuario = new ClienteDomain(entidade.Id, SituacaoDeCliente.ObterPorIdOuNovo(entidade.IdSituacao), entidade.DataHoraCadastro,
            entidade.Nome, entidade.Sobrenome, entidade.Cpf, entidade.DataNascimento, entidade.Email, entidade.EmailConfirmado,
            entidade.Celular, entidade.CelularConfirmado, entidade.Senha);

            usuario.RecuperarSenha(dto.SenhaNova);
            entidade.IdSituacao = usuario.Situacao.Id;
            entidade.Senha = usuario.SenhaCriptografada;

            await usuarioRepository.SaveChangesAsync();
            return new ProjecaoDeUsuarioLogado()
            {
                Token = tokenService.GerarToken(usuario.Id, usuario.Nome, usuario.Email, usuario.Celular, EnumTipoDeUsuario.Cliente, usuario.Situacao.Id),
                Usuario = new ProjecaoDeUsuario(usuario)
            };
        }
        else if (usuarioAutenticado.TipoDeUsuario == EnumTipoDeUsuario.Gestor)
        {
            var entidade = await usuarioRepository.UnitOfWork.Gestors.FirstOrDefaultAsync(u => u.Id == usuarioAutenticado.Id) ?? throw new GestorNaoEncontradoException();

            var usuario = new Gestor(entidade.Id, SituacaoDeGestor.ObterPorIdOuNovo(entidade.IdSituacao), entidade.DataHoraCadastro, entidade.Nome, entidade.Sobrenome, entidade.Cpf, entidade.DataNascimento, entidade.Email, entidade.EmailConfirmado, entidade.Senha);

            usuario.RecuperarSenha(dto.SenhaNova);
            entidade.IdSituacao = usuario.Situacao.Id;
            entidade.Senha = usuario.SenhaCriptografada;

            await usuarioRepository.SaveChangesAsync();
            return new ProjecaoDeUsuarioLogado()
            {
                Token = tokenService.GerarToken(usuario.Id, usuario.Nome, usuario.Email, string.Empty, EnumTipoDeUsuario.Gestor, usuario.Situacao.Id),
                Usuario = new ProjecaoDeUsuario(usuario)
            };
        }
        else
        {
            throw new UsuarioNaoAutorizadoException();
        }
    }

    public async Task AlterarSenha(UsuarioAutenticado usuarioAutenticado, DtoDeAlteracaoDeSenha dto)
    {
        if (usuarioAutenticado.TipoDeUsuario == EnumTipoDeUsuario.Cliente)
        {
            var entidade = await usuarioRepository.UnitOfWork.Clientes.FirstOrDefaultAsync(c => c.Id == usuarioAutenticado.Id) ?? throw new ClienteNaoEncontradoException();

            var usuario = new ClienteDomain(entidade.Id, SituacaoDeCliente.ObterPorIdOuNovo(entidade.IdSituacao), entidade.DataHoraCadastro,
                entidade.Nome, entidade.Sobrenome, entidade.Cpf, entidade.DataNascimento, entidade.Email, entidade.EmailConfirmado,
                entidade.Celular, entidade.CelularConfirmado, entidade.Senha);

            if (!usuario.SenhaEhValida(dto.SenhaAtual))
                throw new SenhaIncorretaException();

            usuario.AtualizarSenha(dto.SenhaNova);
            entidade.Senha = usuario.SenhaCriptografada;
            await usuarioRepository.SaveChangesAsync();
        }
        else
        {
            var entidade = await usuarioRepository.UnitOfWork.Gestors.FirstOrDefaultAsync(g => g.Id == usuarioAutenticado.Id) ?? throw new GestorNaoEncontradoException();

            var usuario = new Gestor(entidade.Id, SituacaoDeGestor.ObterPorIdOuNovo(entidade.IdSituacao), entidade.DataHoraCadastro, entidade.Nome, entidade.Sobrenome, entidade.Cpf, entidade.DataNascimento, entidade.Email, entidade.EmailConfirmado, entidade.Senha);

            if (!usuario.SenhaEhValida(dto.SenhaAtual))
                throw new SenhaIncorretaException();

            usuario.AtualizarSenha(dto.SenhaNova);
            entidade.Senha = usuario.SenhaCriptografada;
            await usuarioRepository.SaveChangesAsync();
        }
    }
}
