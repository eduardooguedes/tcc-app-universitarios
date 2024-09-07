using Dashdine.Service.Models.Pedido;

namespace Dashdine.Service.Models.Cliente.Pedido;

public sealed record ProjecaoDePedidoDoClienteParaListagem(
    Guid Id,
    string? DataHoraARetirar,
    ProjecaoDeSituacaoDoPedido Situacao,
    decimal ValorTotal,
    ProjecaoDeLocalizacaoDoClienteParaListagemDePedido LocalizacaoDoCliente,
    int? DistanciaEmMetrosAteEstabelecimento,
    IEnumerable<ProjecaoDeProdutoDoPedidoParaListagem> Produtos,
    ProjecaoDeEstabelecimentoDoPedidoDoCliente Estabelecimento,
    string? MensagemParaRetirar);