using Dashdine.Service.Models.Cliente.Pedido;
using Dashdine.Service.Models.Pedido;

namespace Dashdine.Service.Models.Estabelecimento.Pedido;

public sealed record ProjecaoDePedidoDoDia(Guid Id, int? Numero, TimeOnly HorarioDeRetirada, ProjecaoDeSituacaoDoPedido Situacao, ProjecaoDeClienteParaPedidoDoDia Cliente, decimal ValorTotal, IEnumerable<ProjecaoDeProdutoDoPedidoParaListagem> Produtos, string? ObservacaoDoCliente, string? ObservacaoDoEstabelecimento, bool PermitidoAceitar, bool PermitidoRejeitar, bool PermitidoEntregar, bool PermitidoDesfazerEntrega);
