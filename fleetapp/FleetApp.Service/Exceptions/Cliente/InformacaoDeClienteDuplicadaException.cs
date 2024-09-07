namespace Dashdine.Service.Exceptions.Cliente;

public sealed class InformacaoDeClienteDuplicadaException(string informacaoDuplicada) : ServiceException($"{informacaoDuplicada} já vinculado a um cliente.");