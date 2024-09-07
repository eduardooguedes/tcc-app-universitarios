namespace Dashdine.Service.Models.Cliente;

public class ProjecaoDeClienteLogado
{
    /// <summary>
    /// Token de autenticação
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// Informações do cliente logado.
    /// </summary>
    public ProjecaoDeCliente Cliente { get; set; }
}
