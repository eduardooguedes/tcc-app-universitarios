using Dashdine.Application.Attributes;
using Dashdine.Application.Controllers.Shared;
using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Service.Exceptions;
using Dashdine.Service.Interface.Cliente;
using Dashdine.Service.Models.Cliente.Pedido;
using Dashdine.Service.Models.Filtros;
using Dashdine.Service.Models.Pedido;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashdine.Application.Controllers.Cliente;

[Route("v1/cliente/pedido")]
[Authorize(Roles = Roles.Roles.RoleCliente)]
public sealed class PedidosClienteController(IPedidoClienteService pedidoClienteService, IPagamentoPedidoClienteService pagamentoPedidoClienteService) : ApiController
{

    /// <summary>
    /// Retorna informações sobre os pedidos do cliente.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    [Route("informacoes")]
    public async Task<IActionResult> ObterInformacoesSobreOsPedidos()
    {
        try
        {
            return ResponseOk(await pedidoClienteService.ObterInformacoesSobreOsPedidosDoCliente(new UsuarioAutenticado(User).Id));
        }
        catch (ServiceException domainEx)
        {
            return ResponseBadRequest(domainEx.Message);
        }
        catch (Exception ex)
        {
            return ResponseInternalServerError(ex.GetBaseException().Message);
        }
    }

    /// <summary>
    /// Retornar pedidos do cliente.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    [Route("listar")]
    public async Task<IActionResult> ObterPedidos([FromQuery] FiltrosRequest? filtros = null, [FromQuery] string? situacoesSeparadasPorVirgula = "")
    {
        try
        {
            var listaStatus = new List<int>();
            if (!string.IsNullOrEmpty(situacoesSeparadasPorVirgula))
            {
                foreach (var status in situacoesSeparadasPorVirgula.Split(','))
                {
                    if (!int.TryParse(status, out int statusInt))
                        return ResponseBadRequest("Status inválido informado.");
                    listaStatus.Add(statusInt);
                }
            }

            return ResponseOk(await pedidoClienteService.ObterPedidosDoCliente(filtros, new UsuarioAutenticado(User).Id, listaStatus));
        }
        catch (ServiceException domainEx)
        {
            return ResponseBadRequest(domainEx.Message);
        }
        catch (Exception ex)
        {
            return ResponseInternalServerError(ex.GetBaseException().Message);
        }
    }

    /// <summary>
    /// Retornar pedidos retirados pelo cliente.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    [Route("listarRetirados")]
    public async Task<IActionResult> ObterPedidosRetirados([FromQuery] FiltrosRequest? filtros = null)
    {
        try
        {
            return ResponseOk(await pedidoClienteService.ObterPedidosDoCliente(filtros, new UsuarioAutenticado(User).Id, new List<int>(), true));
        }
        catch (ServiceException domainEx)
        {
            return ResponseBadRequest(domainEx.Message);
        }
        catch (Exception ex)
        {
            return ResponseInternalServerError(ex.GetBaseException().Message);
        }
    }

    /// <summary>
    /// Retornar pedido do cliente a partir do Id.
    /// </summary>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    [Route("{idPedido}")]
    public async Task<IActionResult> ObterPedido([FromRoute] Guid idPedido)
    {
        try
        {
            return ResponseOk(await pedidoClienteService.ObterPedidoDoCliente(new UsuarioAutenticado(User), idPedido));
        }
        catch (ServiceException domainEx)
        {
            return ResponseBadRequest(domainEx.Message);
        }
        catch (Exception ex)
        {
            return ResponseInternalServerError(ex.GetBaseException().Message);
        }
    }

    /// <summary>
    /// Retornar pedidos do cliente.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    [Route("situacoesListadas")]
    public async Task<IActionResult> ObterSituacoesListadasParaCliente()
    {
        try
        {
            return ResponseOk(await pedidoClienteService.ObterSituacoesListadasParaCliente());
        }
        catch (ServiceException domainEx)
        {
            return ResponseBadRequest(domainEx.Message);
        }
        catch (Exception ex)
        {
            return ResponseInternalServerError(ex.GetBaseException().Message);
        }
    }

    /// <summary>
    /// Retorna formas de pagamento disponíveis.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    [Route("{idPedido}/formasDePagamento")]
    public async Task<IActionResult> ObterFormasDePagamentoDoPedido([FromRoute] Guid idPedido)
    {
        try
        {
            return ResponseOk(await pedidoClienteService.ObterFormasDePagamentoDoPedido(idPedido));
        }
        catch (ServiceException serviceEx)
        {
            return ResponseBadRequest(serviceEx.Message);
        }
        catch (Exception ex)
        {
            return ResponseInternalServerError(ex.GetBaseException().Message);
        }
    }

    /// <summary>
    /// Adiciona um item ao pedido em andamento do estabelecimento.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Route("emAndamento")]
    public async Task<IActionResult> AdicionarAoPedidoEmAndamento([FromBody] DtoAdicionarAoPedidoDoCliente pedido)
    {
        try
        {
            return ResponseOk(await pedidoClienteService.AdicionarAoPedidoEmAndamento(new UsuarioAutenticado(User), pedido));
        }
        catch (ServiceException domainEx)
        {
            return ResponseBadRequest(domainEx.Message);
        }
        catch (Exception ex)
        {
            return ResponseInternalServerError(ex.GetBaseException().Message);
        }
    }


    /// <summary>
    /// Cria uma solicitação de ajuda para o pedido.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Route("{idPedido}/solicitarAjuda")]
    public async Task<IActionResult> SolicitarAjudaParaPedido([FromRoute] Guid idPedido, [FromBody] DtoSolicitacaoDeAjuda ajuda)
    {
        try
        {
            await pedidoClienteService.SolicitarAjudaParaPedido(new UsuarioAutenticado(User), idPedido, ajuda);
            return ResponseOk();
        }
        catch (ServiceException domainEx)
        {
            return ResponseBadRequest(domainEx.Message);
        }
        catch (Exception ex)
        {
            return ResponseInternalServerError(ex.GetBaseException().Message);
        }
    }

    /// <summary>
    /// Atualiza um pedido.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPut]
    [Route("{idPedido}")]
    public async Task<IActionResult> AtualizarPedido([FromRoute] Guid idPedido, [FromBody] DtoDoPedidoDoCliente pedido)
    {
        try
        {
            await pedidoClienteService.AtualizarPedido(new UsuarioAutenticado(User), idPedido, pedido);
            return ResponseOk(idPedido);
        }
        catch (ServiceException domainEx)
        {
            return ResponseBadRequest(domainEx.Message);
        }
        catch (Exception ex)
        {
            return ResponseInternalServerError(ex.GetBaseException().Message);
        }
    }

    /// <summary>
    /// Cancela ou exclui um pedido, caso possível.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status204NoContent)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpDelete]
    [Route("{idPedido}")]
    public async Task<IActionResult> CancelarPedido([FromRoute] Guid idPedido)
    {
        try
        {
            await pedidoClienteService.CancelarPedido(new UsuarioAutenticado(User), idPedido);
            return ResponseOk();
        }
        catch (ServiceException domainEx)
        {
            return ResponseBadRequest(domainEx.Message);
        }
        catch (Exception ex)
        {
            return ResponseInternalServerError(ex.GetBaseException().Message);
        }
    }

    /// <summary>
    /// Pagar um pedido.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Route("pagar/{idPedido}")]
    public async Task<IActionResult> PagarPedido([FromRoute] Guid idPedido, [FromBody] DtoPagarPedido pagamento)
    {
        try
        {
            return ResponseOk(await pagamentoPedidoClienteService.PagarPedido(new UsuarioAutenticado(User), idPedido, pagamento));
        }
        catch (ServiceException domainEx)
        {
            return ResponseBadRequest(domainEx.Message);
        }
        catch (Exception ex)
        {
            return ResponseInternalServerError(ex.GetBaseException().Message);
        }
    }

    /// <summary>
    /// Atualiza pagamento de um pedido.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Route("atualizarPagamento")]
    public async Task<IActionResult> AtualizarPagamentoPedido([FromBody] DtoPedidoPagSeguro pagamento)
    {
        try
        {
            await pagamentoPedidoClienteService.AtualizarPagamento(pagamento);
            return ResponseOk();
        }
        catch (ServiceException domainEx)
        {
            return ResponseBadRequest(domainEx.Message);
        }
        catch (Exception ex)
        {
            return ResponseInternalServerError(ex.GetBaseException().Message);
        }
    }

    /// <summary>
    /// Obter pagamento do pedido.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    [Route("pagamento/{idPedido}")]
    public async Task<IActionResult> ObterPagamentoDoPedido([FromRoute] Guid idPedido)
    {
        try
        {
            return ResponseOk(await pagamentoPedidoClienteService.ObterUltimoPagamentoDoPedido(new UsuarioAutenticado(User), idPedido));
        }
        catch (ServiceException domainEx)
        {
            return ResponseBadRequest(domainEx.Message);
        }
        catch (Exception ex)
        {
            return ResponseInternalServerError(ex.GetBaseException().Message);
        }
    }

    /// <summary>
    /// Cancela o pagamento feito para o pedido, caso possível.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpDelete]
    [Route("pagamento/{idPedido}")]
    public async Task<IActionResult> CancelarPagamentoDoPedido([FromRoute] Guid idPedido)
    {
        try
        {
            await pagamentoPedidoClienteService.CancelarPagamentoExcluirPedido(new UsuarioAutenticado(User), idPedido);
            return ResponseOk();
        }
        catch (ServiceException domainEx)
        {
            return ResponseBadRequest(domainEx.Message);
        }
        catch (Exception ex)
        {
            return ResponseInternalServerError(ex.GetBaseException().Message);
        }
    }
}
