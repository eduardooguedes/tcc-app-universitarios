using Dashdine.Domain.Domain.Estabelecimento.HorarioDeFuncionamento;

namespace Dashdine.Service.Models.Estabelecimento.HorarioDeFuncionamento;

public sealed record ProjecaoDeHorarioDeFuncionamentoParaListagem(string Id, ProjecaoDeSituacaoDoHorarioDeFuncionamento Situacao, IEnumerable<DiaDaSemana> Dias, TimeOnly InicioHorario, TimeOnly FimHorario, int IntervaloEmMinutosEntreRetiradas, int? PedidosPorRetirada, int? MinutosEntrePedirERetirar);
