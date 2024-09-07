namespace Dashdine.Service.Exceptions.Gestor;

public sealed class GestorNaoPossuiEstabelecimentoException : ServiceException
{
    public GestorNaoPossuiEstabelecimentoException() : base("Gestor não possui estabelecimento.") {}
}
