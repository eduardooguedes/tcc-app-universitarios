using Dashdine.Service.Exceptions;

namespace Dashdine.Service.Exceptions.Cliente.Pedido;

public class QuantidadeInformadaMaiorQuePermitidaParaAdicionalException : ServiceException
{
    public QuantidadeInformadaMaiorQuePermitidaParaAdicionalException(string nome, string nomeProduto, int quantidadeMaxima) : base($"Permitido até {quantidadeMaxima} unidade{(quantidadeMaxima == 1 ? "" : "s")} do adicional {nome} no produto {nomeProduto}.") { }
}