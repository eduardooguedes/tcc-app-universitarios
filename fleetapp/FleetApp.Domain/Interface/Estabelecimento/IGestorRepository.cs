using Dashdine.Domain.Domain.Estabelecimento;
using Dashdine.Domain.Interfafe;

namespace Dashdine.Domain.Interface.Estabelecimento;

public interface IGestorRepository : IBaseRepository<Entitys.Gestor>
{
    Task<string> ObterInformacaoUnicaJaCadastrada(Domain.Estabelecimento.Gestor.Gestor gestor);
    Task Novo(Domain.Estabelecimento.Gestor.Gestor gestor);
    Task<Domain.Estabelecimento.Gestor.Gestor?> ObterPorEmail(string email);
    Task<Domain.Estabelecimento.Gestor.Gestor?> ObterPorId(Guid id);
    Task<Domain.Estabelecimento.Gestor.Gestor?> ObterPorIdComEstabelecimento(Guid id);
    Task<List<EstabelecimentosDoGestor>?> ObterListaDeEstabelecimentosDoGestor(Guid idGestor);
}
