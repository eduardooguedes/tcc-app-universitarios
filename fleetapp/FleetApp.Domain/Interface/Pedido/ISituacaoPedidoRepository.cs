using Dashdine.Domain.Domain.Pedido;

namespace Dashdine.Domain.Interface.Pedido;

public interface ISituacaoPedidoRepository
{
    Task<IEnumerable<SituacaoDoPedidoDomain>> ObterSituacoesListadasParaCliente();
    Task<IEnumerable<SituacaoDoPedidoDomain>> ObterSituacoes();
    Task<SituacaoDoPedidoDomain> ObterSituacaoEmAndamento();
    Task<SituacaoDoPedidoDomain> ObterSituacaoCancelado();
    Task<SituacaoDoPedidoDomain> ObterSituacaoAceito();
    Task<SituacaoDoPedidoDomain> ObterSituacaoAguardandoPagamento();
    Task<SituacaoDoPedidoDomain> ObterSituacaoRetirado();
    Task<SituacaoDoPedidoDomain> ObterSituacaoRejeitado();
    Task<SituacaoDoPedidoDomain> ObterSituacaoNovo();
}
