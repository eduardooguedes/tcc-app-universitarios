namespace Dashdine.Service.Exceptions.Estabelecimento;

public sealed class EstabelecimentoNaoEncontradoException : ServiceException
{
    public EstabelecimentoNaoEncontradoException() : base("Estabelecimento não encontrado.") { }
}
