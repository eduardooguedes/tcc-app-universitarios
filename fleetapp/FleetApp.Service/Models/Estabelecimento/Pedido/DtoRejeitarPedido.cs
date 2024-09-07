using System.ComponentModel.DataAnnotations;

namespace Dashdine.Service.Models.Estabelecimento.Pedido;

public sealed record DtoRejeitarPedido([Required(ErrorMessage = "Informe o motivo da rejeição")][StringLength(150, MinimumLength = 20, ErrorMessage = "Informe um motivo entre 20 e 150 caracteres")] string Motivo);