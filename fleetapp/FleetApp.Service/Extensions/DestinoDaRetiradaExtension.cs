using Dashdine.Domain.Domain.Estabelecimento.HorarioDeFuncionamento;
using Dashdine.Service.Models.Cliente.Estabelecimento;

namespace Dashdine.Service.Extensions;

public static class DestinoDaRetiradaExtension
{
    public static ProjecaoDeDestinoDaRetiradaDoHorarioDeFuncionamento AsProjecao(this DestinoDaRetiradaDoPedidoDomain destino) => new(destino.Id, destino.Descricao);
}
