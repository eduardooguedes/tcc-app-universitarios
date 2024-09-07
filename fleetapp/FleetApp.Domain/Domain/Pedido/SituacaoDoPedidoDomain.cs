﻿namespace Dashdine.Domain.Domain.Pedido;

public sealed record SituacaoDoPedidoDomain(int Id, string Descricao, bool ListadoParaCliente, bool ListadoParaGestor, bool PossuiCategoriaPropriaParaCliente, bool PermitidoCancelar, int? IntervaloMinimoEmHorasAntesDeRetirarParaCancelar, bool ApagarPedidoAoCancelar, bool PermitidoSolicitarAjuda, bool PermitidoRetirar, bool PermitidoPedirNovamente, bool VisualizaMotivo, string CorHexadecimal, bool PermitidoRejeitar, bool PermitidoPagar, bool EstornarPagamentoAoCancelar);