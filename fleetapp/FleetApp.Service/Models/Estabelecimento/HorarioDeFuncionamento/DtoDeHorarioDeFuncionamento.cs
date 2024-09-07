using System.ComponentModel.DataAnnotations;

namespace Dashdine.Service.Models.Estabelecimento.HorarioDeFuncionamento;

public class DtoDeHorarioDeFuncionamento
{
    /// <summary>
    /// Informar null na inserção.
    /// </summary>
    public Guid? Id { get; set; } = null;

    /// <summary>
    /// Dias da semana. [ 0 = Domingo | 1 = Segunda | 2 = Terça | 3 = Quarta | 4 = Quinta | 5 = Sexta | 6 = Sabado ]
    /// </summary>
    [Required(ErrorMessage = "Obrigatório informar dia(s) da semana.")]
    public List<int> Dias { get; set; }

    /// <summary>
    /// Inicio do horário de funcionamento.
    /// </summary>
    [Required(ErrorMessage = "Obrigatório informar início do horário de funcionamento.")]
    public TimeOnly InicioHorario { get; set; }

    /// <summary>
    /// Fim do horário de funcionamento.
    /// </summary>
    [Required(ErrorMessage = "Obrigatório informar fim do horário de funcionamento.")]
    public TimeOnly FimHorario { get; set; }

    /// <summary>
    /// Destinos da retirada daquele horário de funcionamento. [ 1 = Para levar | 2 = Comer no local ] 
    /// </summary>
    public List<int> DestinosDaRetirada { get; set; }

    /// <summary>
    /// Intervalo entre uma retirada e outra.
    /// Exemplo: Se horario tem inicio as 19:00 e possui intervalo de 10 minutos entre retiradas, existirão as seguintes retiradas: 19:00, 19:10, 19:20...
    /// </summary>
    [Required(ErrorMessage = "Obrigatório informar intervalo em minutos entre retiradas.")]
    public int IntervaloEmMinutosEntreRetiradas { get; set; }

    /// <summary>
    /// Quantidade máxima de pedidos que serão retirados por retirada.
    /// </summary>
    public int? QuantidadePadraoDePedidosPorRetirada { get; set; }

    /// <summary>
    /// Quantidade máxima de pedidos que serão retirados na primeira retirada do horário de funcionamento.
    /// </summary>
    public int? QuantidadeDePedidosDaPrimeiraRetirada { get; set; }

    /// <summary>
    /// Quantidade de produtos do tipo "Preparados" por pedido de cliente.
    /// </summary>
    public int? QuantidadeDeProdutosPreparadosPorPedido { get; set; }

    /// <summary>
    /// Tempo mínimo em minutos entre pedir e retirar produto. Caso seja nulo, será utilizado o 'tempo de preparo' do produto para escolha do horário de retirada pelo cliente.
    /// </summary>
    public int? MinutosEntrePedirERetirar { get; set; }
}
