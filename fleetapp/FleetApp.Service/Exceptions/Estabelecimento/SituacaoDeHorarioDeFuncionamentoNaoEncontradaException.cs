namespace Dashdine.Service.Exceptions.Estabelecimento;

public sealed class SituacaoDeHorarioDeFuncionamentoNaoEncontradaException : ServiceException
{
    public SituacaoDeHorarioDeFuncionamentoNaoEncontradaException() : base("Situação de horário de funcionamento não encontrada.") { }
}
