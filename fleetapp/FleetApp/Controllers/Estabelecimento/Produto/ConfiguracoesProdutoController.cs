using Dashdine.Application.Attributes;
using Dashdine.Application.Controllers.Shared;
using Dashdine.Service.Exceptions;
using Dashdine.Service.Interface.Produto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashdine.Application.Controllers.Produto;

/// <summary>
/// Rota para categorias de produto.
/// </summary>
[Route("v1/estabelecimento/configuracoesDeProduto")]
[Authorize]
public class ConfiguracoesProdutoController : ApiController
{
    private readonly IConfiguracoesProdutoService configuracoesService;

    public ConfiguracoesProdutoController(IConfiguracoesProdutoService configuracoesService)
    {
        this.configuracoesService = configuracoesService;
    }

    /// <summary>
    /// Retorna categorias de produto.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    [Route("")]
    [Authorize(Roles = Roles.Roles.RoleGestor)]
    [ResponseCache(CacheProfileName = "default", Duration = 60 * 60 * 2)]
    public async Task<IActionResult> ObterTodas()
    {
        try
        {
            return ResponseOk(await configuracoesService.ObterTodas());
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