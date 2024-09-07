namespace Dashdine.Service.Exceptions.Estabelecimento;

public sealed class HorarioDeFuncionamentoNaoEncontradoException : ServiceException
{
    public HorarioDeFuncionamentoNaoEncontradoException() : base("Horário de funcionamento não encontrado.") { }
}
