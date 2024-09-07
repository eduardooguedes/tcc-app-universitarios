namespace Dashdine.Domain.Domain.Pedido.Cliente;

public sealed record EstabelecimentoDoPedidoDoClienteDomain(Guid Id, string? Logo, string Nome, EnderecoEstabelecimento Endereco);