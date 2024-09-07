using Dashdine.Domain.Domain.Integracoes.PagSeguro;
using Newtonsoft.Json;

namespace Dashdine.Infrastructure.Repository.Pagamento;

public sealed class PagSeguroClient : HttpClient
{
    private readonly string endpointPedido;
    private readonly string endpointCancelarPagamento;
    public PagSeguroClient(string urlBase, string authorization, string endpointPedido, string endpointCancelarPagamento)
    {
        Timeout = TimeSpan.FromSeconds(20);
        BaseAddress = new Uri(urlBase);
        DefaultRequestHeaders.Add("Authorization", authorization);
        DefaultRequestHeaders.Add("Content", "application/json");
        this.endpointPedido = endpointPedido;
        this.endpointCancelarPagamento = endpointCancelarPagamento;
    }

    public async Task<HttpResponseMessage> SalvarPedidoQrCode(PedidoPagSeguroDomain pedidoPagSeguro) => await PostAsync(endpointPedido, new StringContent(JsonConvert.SerializeObject(pedidoPagSeguro), encoding: System.Text.Encoding.UTF8, "application/json"));

    public async Task<HttpResponseMessage> CancelarPagamentoPedido(string cobrancaGateway, object quantia) => await PostAsync(string.Format(endpointCancelarPagamento, cobrancaGateway), new StringContent(JsonConvert.SerializeObject(quantia), encoding: System.Text.Encoding.UTF8, "application/json"));

    public async Task<HttpResponseMessage> ObterPedidoAtualizado(string idPedidoGateway) => await GetAsync($"{endpointPedido}/{idPedidoGateway}");
}
