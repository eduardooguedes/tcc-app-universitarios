﻿namespace Dashdine.Service.Models.Cliente.Estabelecimento;

public sealed record ProjecaoDeHorarioDeFuncionamentoDoDia(string Horario, IEnumerable<ProjecaoDeDestinoDaRetiradaDoHorarioDeFuncionamento> Destinos);
