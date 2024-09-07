using Dashdine.Domain.Domain.Estabelecimento.HorarioDeFuncionamento;

namespace Dashdine.Service.Models.Estabelecimento.HorarioDeFuncionamento;

public sealed record ProjecaoDeHorarioDeFuncionamento(string Id, IEnumerable<DiaDaSemana> Dias, TimeOnly InicioHorario, TimeOnly FimHorario, IEnumerable<DestinoDaRetiradaDoPedidoDomain> DestinosDaRetirada, int IntervaloEmMinutosEntreRetiradas, int? PedidosPorRetirada, int? ProdutosDoTipoPreparadosPorPedido, int? QuantidadeDePedidosPrimeiraRetirada, int? MinutosEntrePedirERetirar);
