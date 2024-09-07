namespace Dashdine.Service.Exceptions.Gestor;

public sealed class GestorJaVinculadoAOutroEstabelecimentoException : ServiceException
{
    public GestorJaVinculadoAOutroEstabelecimentoException() : base("Gestor já vinculado a outro estabelecimento.") { }
}
