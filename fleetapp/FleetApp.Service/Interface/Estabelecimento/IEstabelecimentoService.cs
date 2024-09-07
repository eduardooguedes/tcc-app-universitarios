using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Service.Models.Estabelecimento;
using Microsoft.AspNetCore.Http;

namespace Dashdine.Service.Interface.Estabelecimento;

public interface IEstabelecimentoService
{
    Task<Guid> Cadastrar(UsuarioAutenticado gestor, DtoDeEstabelecimento dto);
    Task Editar(UsuarioAutenticado gestor, Guid idEstabelecimento, DtoDeEstabelecimentoEdicao estabelecimento);
    Task EditarEnderecoRetirada(UsuarioAutenticado gestor, Guid idEstabelecimento, Guid idEndereco, DtoDeEnderecoRetirada endereco);
    Task<ProjecaoDeEstabelecimento> Obter(UsuarioAutenticado gestor, Guid idEstabelecimento);
    Task<ProjecaoDeInformacoesDoEstabelecimento> ObterInformacoes(UsuarioAutenticado gestor, Guid idEstabelecimento);
    Task<string> AtualizarLogo(UsuarioAutenticado usuarioAutenticado, Guid idEstabelecimento, IFormFile logo);
    Task AtualizarSituacao(UsuarioAutenticado usuarioAutenticado, Guid idEstabelecimento, int novaSituacao);
    Task RemoverLogo(UsuarioAutenticado usuarioAutenticado, Guid idEstabelecimento);
}
