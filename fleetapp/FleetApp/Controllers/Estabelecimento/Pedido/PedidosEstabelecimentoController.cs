using Dashdine.Application.Attributes;
using Dashdine.Application.Controllers.Shared;
using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Service.Exceptions;
using Dashdine.Service.Interface.Estabelecimento;
using Dashdine.Service.Models.Estabelecimento.Pedido;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashdine.Application.Controllers.Estabelecimento.Pedido;

[Route("v1/estabelecimento")]
[Authorize(Roles = Roles.Roles.RoleGestor)]
public sealed class PedidosEstabelecimentoController(IPedidoEstabelecimentoService pedidoEstabelecimentoService) : ApiController
{
    /// <summary>
    /// Resumo mensal de pedidos do estabelecimento.
    /// </summary>
    /// <param name="idEstabelecimento">Estabelecimento do resumo.</param>
    /// <param name="mes">Opcional. Padrão: mes atual.</param>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    [Route("{idEstabelecimento}/pedido/resumoMensal")]
    public async Task<IActionResult> ObterResumoDoMes([FromRoute] Guid idEstabelecimento, [FromQuery] DateTime? mes = null)
    {
        try
        {
            return ResponseOk(await pedidoEstabelecimentoService.ObterResumoDoMes(new UsuarioAutenticado(User), idEstabelecimento, mes));
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
    /// Pedidos do dia informado.
    /// </summary>
    /// <param name="idEstabelecimento">Estabelecimento.</param>
    /// <param name="data">Dia dos pedidos</param>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    [Route("{idEstabelecimento}/pedido/{data}")]
    public async Task<IActionResult> ObterPedidosDoDia([FromRoute] Guid idEstabelecimento, [FromRoute] DateTime data)
    {
        try
        {
            return ResponseOk(await pedidoEstabelecimentoService.ObterPedidosDoDia(new UsuarioAutenticado(User), idEstabelecimento, DateOnly.FromDateTime(data)));
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
    /// Aceita um pedido.
    /// </summary>
    /// <param name="idEstabelecimento">Estabelecimento</param>
    /// <param name="idPedido">Pedido</param>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Route("{idEstabelecimento}/pedido/{idPedido}/aceitar")]
    public async Task<IActionResult> AceitarPedido([FromRoute] Guid idEstabelecimento, [FromRoute] Guid idPedido)
    {
        try
        {
            return ResponseOk(await pedidoEstabelecimentoService.AceitarPedido(new UsuarioAutenticado(User), idEstabelecimento, idPedido));
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
    /// Entrega um pedido.
    /// </summary>
    /// <param name="idEstabelecimento">Estabelecimento</param>
    /// <param name="idPedido">Pedido</param>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Route("{idEstabelecimento}/pedido/{idPedido}/entregar")]
    public async Task<IActionResult> EntregarPedido([FromRoute] Guid idEstabelecimento, [FromRoute] Guid idPedido)
    {
        try
        {
            return ResponseOk(await pedidoEstabelecimentoService.EntregarPedido(new UsuarioAutenticado(User), idEstabelecimento, idPedido));
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
    /// Rejeita um pedido ou atualiza a rejeição.
    /// </summary>
    /// <param name="idEstabelecimento">Estabelecimento.</param>
    /// <param name="idPedido">Pedido</param>
    /// <param name="rejeitarPedido">Informações para rejeição</param>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Route("{idEstabelecimento}/pedido/{idPedido}/rejeitar")]
    public async Task<IActionResult> RejeitarPedido([FromRoute] Guid idEstabelecimento, [FromRoute] Guid idPedido, [FromBody] DtoRejeitarPedido rejeitarPedido)
    {
        try
        {
            return ResponseOk(await pedidoEstabelecimentoService.RejeitarPedido(new UsuarioAutenticado(User), idEstabelecimento, idPedido, rejeitarPedido));
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
    /// Desfazer entrega de um pedido.
    /// </summary>
    /// <param name="idEstabelecimento">Estabelecimento</param>
    /// <param name="idPedido">Pedido</param>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Route("{idEstabelecimento}/pedido/{idPedido}/desfazerEntrega")]
    public async Task<IActionResult> DesfazerEntregaDoPedido([FromRoute] Guid idEstabelecimento, [FromRoute] Guid idPedido)
    {
        try
        {
            return ResponseOk(await pedidoEstabelecimentoService.DesfazerEntregaDoPedido(new UsuarioAutenticado(User), idEstabelecimento, idPedido));
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
