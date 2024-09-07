namespace Dashdine.Service.Exceptions.Estabelecimento;

public sealed class SituacaoDeEstabelecimentoNaoEncontradaException : ServiceException
{
    public SituacaoDeEstabelecimentoNaoEncontradaException() : base("Situação de estabelecimento não encontrada.") { }
}
