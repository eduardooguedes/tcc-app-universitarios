using Dashdine.Application.Attributes;
using Dashdine.Application.Controllers.Shared;
using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Service.Exceptions;
using Dashdine.Service.Interface.Produto;
using Dashdine.Service.Models.Produto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Dashdine.Application.Controllers.Produto;

/// <summary>
/// Rota para adicional de produto.
/// </summary>
[Route("v1/estabelecimento/adicionalDeProduto")]
[Authorize]
public class AdicionalDeProdutoController(IAdicionalDeProdutoService adicionalService) : ApiController
{
    /// <summary>
    /// Cadastra um adicional.
    /// </summary>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Route("")]
    [Authorize(Roles = Roles.Roles.RoleGestor)]
    public async Task<IActionResult> Cadastrar([FromBody] DtoDeAdicional dtoAdicional)
    {
        try
        {
            return ResponseOk(await adicionalService.Cadastrar(new UsuarioAutenticado(User), dtoAdicional));
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
    /// Atualiza um adicional.
    /// </summary>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPut]
    [Route("{id}")]
    [Authorize(Roles = Roles.Roles.RoleGestor)]
    public async Task<IActionResult> Atualizar([FromRoute] Guid id, [FromBody] DtoDeAdicional dtoAdicional)
    {
        try
        {
            await adicionalService.Atualizar(new UsuarioAutenticado(User), id, dtoAdicional);
            return ResponseOk();
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
    /// Edita a situação de um adicional do estabelecimento do gestor logado.
    /// </summary>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPut]
    [Route("{id}/situacao/{idNovaSituacao}")]
    [Authorize(Roles = Roles.Roles.RoleGestor)]
    public async Task<IActionResult> AtualizarSituacao([FromRoute] Guid id, [FromRoute] int idNovaSituacao)
    {
        try
        {
            await adicionalService.AtualizarSituacao(new UsuarioAutenticado(User), id, idNovaSituacao);
            return ResponseOk();
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
    /// Retorna adicional e lista de produtos vinculados ao adicional.
    /// </summary>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    [Route("{id}")]
    [Authorize(Roles = Roles.Roles.RoleGestor)]
    public async Task<IActionResult> ObterPorId([FromRoute] Guid id)
    {
        try
        {
            return ResponseOk(await adicionalService.GestorObterPorId(new UsuarioAutenticado(User), id));
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
    /// Retorna todos os adicionais cadastrados.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    [Route("lista")]
    [Authorize(Roles = Roles.Roles.RoleGestor)]
    public async Task<IActionResult> ObterTodos()
    {
        try
        {
            return ResponseOk(await adicionalService.GestorObterTodos(new UsuarioAutenticado(User)));
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
    /// Retorna todos os adicionais que possuam filtro informado no nome.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    [Route("filtro/{filtroDeNome}")]
    [Authorize(Roles = Roles.Roles.RoleGestor)]
    public async Task<IActionResult> ObterPorFiltroDeNome([FromRoute][Required][StringLength(20, MinimumLength = 4, ErrorMessage = "Utilize um filtro de 4 a 20 caracteres.")] string filtroDeNome)
    {
        try
        {
            return ResponseOk(await adicionalService.GestorObterPorFiltro(filtroDeNome));
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
    /// Remove vinculo entre adicional e produto.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpDelete]
    [Route("{id}/produto/{idProduto}")]
    [Authorize(Roles = Roles.Roles.RoleGestor)]
    public async Task<IActionResult> RemoverVinculoEntreAdicionalEProduto([FromRoute] Guid id, [FromRoute] Guid idProduto)
    {
        try
        {
            await adicionalService.RemoverVinculoEntreAdicionalEProduto(new UsuarioAutenticado(User), id, idProduto);
            return ResponseOk();
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
}
