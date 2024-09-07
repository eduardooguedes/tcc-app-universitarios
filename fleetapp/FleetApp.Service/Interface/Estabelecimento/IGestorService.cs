using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Service.Models.Estabelecimento.Gestor;

namespace Dashdine.Service.Interface.Estabelecimento;

public interface IGestorService
{
    Task<ProjecaoDeGestor> Obter(Guid idGestor);
    Task<ProjecaoDeGestorLogado> Novo(DtoDeGestor gestor);
    Task Editar(Guid idUsuario, DtoDeEdicaoDeGestor dtoGestor);
}
