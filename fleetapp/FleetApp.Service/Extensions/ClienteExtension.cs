using Dashdine.Service.Models.Cliente;

namespace Dashdine.Service.Extensions;

public static class ClienteExtension
{
    public static ProjecaoDeCliente AsProjecao(this Domain.Domain.Cliente.ClienteDomain domain) => new(domain.Id.ToString(), domain.Situacao.Id, domain.CPF, domain.Nome, domain.Sobrenome, domain.Email, domain.EmailConfirmado, domain.Celular, domain.CelularConfirmado, domain.DataHoraCadastro, domain.DataDeNascimento, null);
}
