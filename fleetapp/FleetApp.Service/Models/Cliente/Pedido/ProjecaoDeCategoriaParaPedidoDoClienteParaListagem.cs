﻿namespace Dashdine.Service.Models.Cliente.Pedido;

public sealed record ProjecaoDeCategoriaParaPedidoDoClienteParaListagem(string Categoria, IEnumerable<ProjecaoDePedidoDoClienteParaListagem> Pedidos);