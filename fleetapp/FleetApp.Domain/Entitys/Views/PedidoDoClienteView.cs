using System.ComponentModel.DataAnnotations.Schema;

namespace Dashdine.Domain.Entitys.Views;

public class PedidoDoClienteView
{
    [Column("id")]
    public Guid Id { get; set; }

    [Column("numero")]
    public int? Numero { get; set; }

    [Column("id_cliente")]
    public Guid IdCliente { get; set; }

    [Column("nome_cliente")]
    public string NomeCliente { get; set; }

    [Column("cpf_cliente")]
    public string CpfCliente { get; set; }

    [Column("email_cliente")]
    public string EmailCliente { get; set; }

    [Column("celular_cliente")]
    public string CelularCliente { get; set; }

    [Column("data_hora")]
    public DateTime DataHora { get; set; }

    [Column("data_hora_a_retirar")]
    public DateTime? DataHoraARetirar { get; set; }

    [Column("observacao")]
    public string? Observacao { get; set; }

    [Column("observacao_estabelecimento")]
    public string? ObservacaoEstabelecimento { get; set; }

    [Column("valor_total")]
    public decimal ValorTotal { get; set; }

    [Column("data_hora_retirado")]
    public DateTime? DataHoraRetirado { get; set; }

    #region Situacao
    [Column("id_situacao_pedido")]
    public int IdSituacaoPedido { get; set; }

    [Column("descricao_situacao_pedido")]
    public string DescricaoSituacaoPedido { get; set; }

    [Column("situacao_em_andamento")]
    public bool SituacaoEmAndamento { get; set; }

    [Column("situacao_listada_para_cliente")]
    public bool SituacaoListadaParaCliente { get; set; }

    [Column("situacao_listada_para_gestor")]
    public bool SituacaoListadaParaGestor { get; set; }

    [Column("situacao_possui_categoria_propria")]
    public bool SituacaoPossuiCategoriaPropria { get; set; }

    [Column("situacao_permitido_cancelar")]
    public bool SituacaoPermitidoCancelar { get; set; }

    [Column("situacao_intervalo_minimo_em_horas")]
    public int? SituacaoIntervaloMinimoEmHorasAntesDeRetirarParaCancelar { get; set; }

    [Column("situacao_apagar_pedido_ao_cancelar")]
    public bool SituacaoApagarPedidoAoCancelar { get; set; }

    [Column("situacao_permitido_solicitar_ajuda")]
    public bool SituacaoPermitidoSolicitarAjuda { get; set; }

    [Column("situacao_permitido_retirar")]
    public bool SituacaoPermitidoRetirar { get; set; }

    [Column("situacao_permitido_pedir_novamente")]
    public bool SituacaoPermitidoPedirNovamente { get; set; }

    [Column("situacao_visualiza_motivo")]
    public bool SituacaoVisualizaMotivo { get; set; }

    [Column("situacao_cor_hexadecimal")]
    public string SituacaoCorHexadecimal { get; set; }

    [Column("situacao_permitido_rejeitar")]
    public bool SituacaoPermitidoRejeitar { get; set; }

    [Column("situacao_permitido_pagar")]
    public bool SituacaoPermitidoPagar { get; set; }

    [Column("situacao_estornar_pagamento_ao_cancelar")]
    public bool SituacaoEstornarPagamentoAoCancelar { get; set; }

    #endregion
    #region Destino
    [Column("id_destino_pedido")]
    public int? IdDestinoPedido { get; set; }

    [Column("descricao_destino")]
    public string? DescricaoDestino { get; set; }
    #endregion

    #region Localizacao
    [Column("id_localizacao_cliente")]
    public Guid IdLocalizacaoCliente { get; set; }

    [Column("apelido_localizacao_cliente")]
    public string ApelidoLocalizacaoCliente { get; set; }

    [Column("latitude_localizacao_cliente")]
    public decimal LatitudeLocalizacaoCliente { get; set; }

    [Column("longitude_localizacao_cliente")]
    public decimal LongitudeLocalizacaoCliente { get; set; }

    [Column("timezone_localizacao_cliente")]
    public string TimeZoneLocalizacaoCliente { get; set; }
    #endregion

    #region Estabelecimento
    [Column("id_estabelecimento")]
    public Guid IdEstabelecimento { get; set; }

    [Column("logo_estabelecimento")]
    public string LogoEstabelecimento { get; set; }

    [Column("nome_estabelecimento")]
    public string NomeEstabelecimento { get; set; }

    [Column("id_endereco_estabelecimento")]
    public Guid IdEnderecoEstabelecimento { get; set; }

    [Column("cep_endereco_estabelecimento")]
    public string CepEnderecoEstabelecimento { get; set; }

    [Column("logradouro_endereco_estabelecimento")]
    public string LogradouroEnderecoEstabelecimento { get; set; }

    [Column("numero_endereco_estabelecimento")]
    public int NumeroEnderecoEstabelecimento { get; set; }

    [Column("complemento_endereco_estabelecimento")]
    public string ComplementoEnderecoEstabelecimento { get; set; }

    [Column("bairro_endereco_estabelecimento")]
    public string BairroEnderecoEstabelecimento { get; set; }

    [Column("cidade_endereco_estabelecimento")]
    public string CidadeEnderecoEstabelecimento { get; set; }

    [Column("estado_endereco_estabelecimento")]
    public string EstadoEnderecoEstabelecimento { get; set; }

    [Column("latitude_endereco_estabelecimento")]
    public decimal LatitudeEnderecoEstabelecimento { get; set; }

    [Column("longitude_endereco_estabelecimento")]
    public decimal LongitudeEnderecoEstabelecimento { get; set; }

    [Column("timezone_localizacao_estabelecimento")]
    public string TimeZoneLocalizacaoEstabelecimento { get; set; }
    #endregion
}
