namespace Dashdine.Service.Exceptions.Estabelecimento;

public sealed class NaoEhPossivelAtualizarSituacaoException : ServiceException
{
    public NaoEhPossivelAtualizarSituacaoException() : base("Não é possível atualizar essa situação.") { }
}
