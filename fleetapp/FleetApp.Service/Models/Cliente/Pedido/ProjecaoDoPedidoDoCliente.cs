using Dashdine.Domain.Domain.Estabelecimento.HorarioDeFuncionamento;

namespace Dashdine.Service.Models.Cliente.Pedido;

public sealed record ProjecaoDoPedidoDoCliente(
                        Guid Id,
                        ProjecaoDeLocalizacaoDoClienteDoPedido LocalizacaoDoCliente,
                        IEnumerable<ProjecaoDeProdutoDoPedido> Produtos,
                        ProjecaoDeEstabelecimentoDoPedidoDoCliente Estabelecimento,
                        decimal ValorTotal,
                        string? Observacao,
                        string? ObservacaoDoEstabelecimento,
                        bool PermitidoCancelar,
                        bool PermitidoPedirNovamente,
                        bool PermitidoAlterarProdutos,
                        bool PermitidoAlterarHorarioDeRetirada,
                        bool PermitidoAlterarObservacao,
                        bool PermitidoSolicitarAjuda,
                        DateTime? DataHoraARetirar,
                        DestinoDaRetiradaDoPedidoDomain? DestinoPedido);
