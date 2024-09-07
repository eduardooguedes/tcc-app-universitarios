using System.ComponentModel.DataAnnotations;

namespace Dashdine.Service.Models.Cliente;

public class DtoDeCartaoDoCliente
{
    [StringLength(20, ErrorMessage = "Informe um apelido em até 20 caracteres.")]
    public string Apelido { get; set; }

    [Required(ErrorMessage = "Obrigatório informar o número do cartão.")]
    [CreditCard(ErrorMessage = "Número do cartão inválido")]
    public string Numero { get; set; }

    [Required(ErrorMessage = "Obrigatório informar o tipo do cartão.")]
    public string Tipo { get; set; }

    [Required(ErrorMessage = "Obrigatório informar validade do cartão.")]
    public DateOnly Validade { get; set; }

    [Required(ErrorMessage = "Obrigatório informar código de segurança.")]
    public string CodigoDeSeguranca { get; set; }

    [Required(ErrorMessage = "Obrigatório apresentar as informações do titular")]
    public DtoDoTitularDoCartao Titular { get; set; }

    [Required(ErrorMessage = "Obrigatório informar um endereço de cobrança")]
    public string IdEnderecoCobranca { get; set; }
}
