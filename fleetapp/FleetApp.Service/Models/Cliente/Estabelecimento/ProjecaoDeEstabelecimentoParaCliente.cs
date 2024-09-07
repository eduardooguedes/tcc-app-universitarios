namespace Dashdine.Service.Models.Cliente.Estabelecimento;

public sealed record ProjecaoDeEstabelecimentoParaCliente(string Id, string? Logo, string NomeFantasia, int? DistanciaEmMetrosAteEstabelecimento, TimeOnly? ProximoHorarioDeRetirada); 
