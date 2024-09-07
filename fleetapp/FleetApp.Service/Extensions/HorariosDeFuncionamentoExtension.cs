using Dashdine.Domain.Domain.Estabelecimento.HorarioDeFuncionamento;
using Dashdine.Service.Models.Estabelecimento.HorarioDeFuncionamento;

namespace Dashdine.Service.Extensions;

public static class HorariosDeFuncionamentoExtension
{
    public static ProjecaoDeSituacaoDoHorarioDeFuncionamento AsProjecao(this SituacaoDeHorarioDeFuncionamentoDomain domain) => new(domain.Id, domain.Descricao, domain.Ativo);
}