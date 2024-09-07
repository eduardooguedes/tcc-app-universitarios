using Dashdine.CrossCutting.Enums.Usuario;
using Dashdine.Service.Models.Cliente;
using Dashdine.Service.Models.Estabelecimento.Gestor;
using Dashdine.Service.Models.Usuario;

namespace Dashdine.Service.Interface.Usuario;

public interface IAutenticacaoService
{
    Task<ProjecaoDeClienteLogado?> LoginCliente(DtoDeLogin login);
    Task<ProjecaoDeGestorLogado?> LoginGestor(DtoDeLogin login);
    Task<double> SolicitarParaRecuperarSenha(string email, EnumTipoDeUsuario tipoDeUsuario);
    Task<ProjecaoDeUsuarioLogado?> AutorizarRecuperacaoDeSenha(string email, string codigo, EnumTipoDeUsuario tipoDeUsuario);
}
