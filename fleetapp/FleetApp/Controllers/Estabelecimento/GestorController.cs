using Dashdine.Application.Attributes;
using Dashdine.Application.Controllers.Shared;
using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Service.Exceptions;
using Dashdine.Service.Interface.Estabelecimento;
using Dashdine.Service.Models.Estabelecimento.Gestor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashdine.Application.Controllers.Estabelecimento;

/// <summary>
/// Rota para gestor.
/// </summary>
[Route("v1/gestor")]
public class GestorController(IGestorService gestorService) : ApiController
{
    /// <summary>
    /// Cadastra um gestor. Retorna token.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Route("")]
    [AllowAnonymous]
    public async Task<IActionResult> Novo([FromBody] DtoDeGestor gestor)
    {
        try
        {
            var gestorLogado = await gestorService.Novo(gestor);
            return ResponseOk(gestorLogado);
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
    /// Edita um gestor.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPut]
    [Route("")]
    public async Task<IActionResult> Editar([FromBody] DtoDeEdicaoDeGestor usuario)
    {
        try
        {
            await gestorService.Editar(new UsuarioAutenticado(User).Id, usuario);
            return ResponseOk(usuario);
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
    /// Retorna gestor logado.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    [Route("logado")]
    public async Task<IActionResult> ObterGestorLogado()
    {
        try
        {
            return ResponseOk(await gestorService.Obter(new UsuarioAutenticado(User).Id));
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