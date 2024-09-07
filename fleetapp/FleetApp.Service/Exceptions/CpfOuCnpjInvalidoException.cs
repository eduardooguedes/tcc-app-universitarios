namespace Dashdine.Service.Exceptions;

public sealed class CpfOuCnpjInvalidoException(string cpfOuCnpj) : ServiceException($"CPF ou CNPJ {cpfOuCnpj} inválido");