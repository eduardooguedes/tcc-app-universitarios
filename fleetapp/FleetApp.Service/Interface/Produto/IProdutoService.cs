using Dashdine.Domain.Domain.Produto;
using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Service.Models.Produto;
using Microsoft.AspNetCore.Http;

namespace Dashdine.Service.Interface.Produto;

public interface IProdutoService
{
    Task AtualizarSituacao(UsuarioAutenticado usuarioAutenticado, Guid idProduto, int novaSituacao);
    Task<ProjecaoParaListagemDeProduto> Cadastrar(UsuarioAutenticado usuarioAutenticado, DtoDeProduto produto);
    Task<ProjecaoParaListagemDeProduto> Atualizar(UsuarioAutenticado usuarioAutenticado, Guid idProduto, DtoDeProduto produto);
    Task<ProjecaoDeProduto> ObterPorId(UsuarioAutenticado usuarioAutenticado, Guid idProduto);
    Task<IEnumerable<ProjecaoParaListagemDeProduto>> GestorObterTodos(UsuarioAutenticado usuarioAutenticado, int? idCategoria);
    Task<string> AtualizarImagem(UsuarioAutenticado usuarioAutenticado, Guid idProduto, IFormFile imagem);
    Task RemoverImagem(UsuarioAutenticado usuarioAutenticado, Guid idProduto);
}
