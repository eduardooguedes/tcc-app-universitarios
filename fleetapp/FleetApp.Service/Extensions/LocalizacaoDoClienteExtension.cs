using Dashdine.Domain.Domain.Pedido;
using Dashdine.Service.Models.Cliente.Pedido;

namespace Dashdine.Service.Extensions;

public static class LocalizacaoDoClienteExtension
{
    public static ProjecaoDeLocalizacaoDoClienteParaListagemDePedido AsProjecaoParaListagem(this LocalizacaoDoClienteDomain domain) => new(domain.Id, domain.Apelido, domain.Latitude, domain.Longitude);
}
