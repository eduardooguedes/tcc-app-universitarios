namespace Dashdine.Domain.Domain.Cliente;

public sealed class EstabelecimentoParaClienteDomain
{
    public Guid Id { get; }
    public string? Logo { get; }
    public string NomeFantasia { get; }
    public int? DistanciaEmMetrosAteEstabelecimento { get; }
    public TimeOnly? ProximoHorarioDeRetirada { get; }

    public EstabelecimentoParaClienteDomain(Guid id, string? logo, string nomeFantasia, int? distanciaEmMetrosAteEstabelecimento, TimeOnly? proximoHorarioDeRetirada)
    {
        Id = id;
        Logo = logo;
        NomeFantasia = nomeFantasia;
        DistanciaEmMetrosAteEstabelecimento = distanciaEmMetrosAteEstabelecimento;
        ProximoHorarioDeRetirada = proximoHorarioDeRetirada;
    }
}
