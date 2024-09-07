using Dashdine.Domain.Domain.Cliente.Cartao;
using Dashdine.Service.Models.Cliente;

namespace Dashdine.Service.Extensions;

public static class CartaoExtension
{
    public static ProjecaoDeCartaoDoCliente AsProjecao(this CartaoDomain cartao, string? imagem) => new(cartao.Id, imagem, cartao.Apelido, cartao.FinalDoNumero, cartao.Tipo.ToString(), cartao.Validade, cartao.Bandeira);
}