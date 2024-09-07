using Dashdine.Domain.Domain.Produto;
using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Service.Models.Produto;

namespace Dashdine.Service.Interface.Produto;

public interface IAdicionalDeProdutoService
{
    public Task<ProjecaoDeAdicionalParaEdicao> GestorObterPorId(UsuarioAutenticado usuario, Guid idAdicional);
    public Task<IEnumerable<ProjecaoDeAdicionalParaListagem>> GestorObterTodos(UsuarioAutenticado usuario);
    public Task<IEnumerable<ProjecaoDeAdicionalParaFiltro>> GestorObterPorFiltro(string filtroDeNome);
    public Task<Guid> Cadastrar(UsuarioAutenticado usuario, DtoDeAdicional dtoAdicional);
    public Task Atualizar(UsuarioAutenticado usuario, Guid idAdicional, DtoDeAdicional dtoDeAdicional);
    public Task AtualizarSituacao(UsuarioAutenticado usuarioAutenticado, Guid idAdicional, int novaSituacao);
    public Task RemoverVinculoEntreAdicionalEProduto(UsuarioAutenticado usuarioAutenticado, Guid idAdicional, Guid idProduto);
}
