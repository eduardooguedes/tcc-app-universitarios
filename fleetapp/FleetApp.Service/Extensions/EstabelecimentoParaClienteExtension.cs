using Dashdine.Domain.Domain.Cliente;
using Dashdine.Domain.Domain.Pedido.Cliente;
using Dashdine.Service.Models.Cliente.Estabelecimento;
using Dashdine.Service.Models.Cliente.Pedido;

namespace Dashdine.Service.Extensions;

public static class EstabelecimentoParaClienteExtension
{
    public static ProjecaoDeEstabelecimentoParaCliente AsProjecaoDeEstabelecimentoParaCliente(this EstabelecimentoParaClienteDomain estabelecimento) => new(estabelecimento.Id.ToString(), estabelecimento.Logo, estabelecimento.NomeFantasia, estabelecimento.DistanciaEmMetrosAteEstabelecimento, estabelecimento.ProximoHorarioDeRetirada);

    public static ProjecaoDeEstabelecimentoDoPedidoDoCliente AsProjecao(this EstabelecimentoDoPedidoDoClienteDomain domain) => new(domain.Id, domain.Logo, domain.Nome, domain.Endereco.Completo, domain.Endereco.Latitude, domain.Endereco.Longitude);
}