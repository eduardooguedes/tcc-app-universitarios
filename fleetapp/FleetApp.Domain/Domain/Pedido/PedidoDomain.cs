using Dashdine.Domain.Domain.Estabelecimento.HorarioDeFuncionamento;
using Dashdine.Domain.Domain.Pedido.Cliente;

namespace Dashdine.Domain.Domain.Pedido;

public sealed record PedidoDomain(Guid Id, int? Numero, ClienteDoPedidoDomain Cliente, LocalizacaoDoClienteDomain LocalizacaoCliente, EstabelecimentoDoPedidoDoClienteDomain Estabelecimento, SituacaoDoPedidoDomain Situacao, DestinoDaRetiradaDoPedidoDomain? Destino, DateTime DataHora, DateTime? DataHoraARetirar, string? Observacao, string? ObservacaoDoEstabelecimento, decimal ValorTotal, DateTime? DataHoraRetirado, IEnumerable<ProdutoDoPedidoDomain> Produtos);