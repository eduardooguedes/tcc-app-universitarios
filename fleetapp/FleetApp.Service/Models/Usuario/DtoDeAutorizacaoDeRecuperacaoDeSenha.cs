namespace Dashdine.Service.Models.Usuario;

public class DtoDeAutorizacaoDeRecuperacaoDeSenha
{
    /// <summary>
    /// Email para autorizar recuperação de senha
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Codigo para autorizar recuperação de senha
    /// </summary>
    public string CodigoDeRecuperacao { get; set; }
}
