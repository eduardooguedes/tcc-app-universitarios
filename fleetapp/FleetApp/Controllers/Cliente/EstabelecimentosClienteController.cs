using Dashdine.Application.Attributes;
using Dashdine.Application.Controllers.Shared;
using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Service.Exceptions;
using Dashdine.Service.Interface.Cliente;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashdine.Application.Controllers.Cliente;

[Route("v1/cliente")]
[Authorize(Roles = Roles.Roles.RoleCliente)]
public sealed class EstabelecimentosClienteController(IEstabelecimentosClienteService estabelecimentosClienteService) : ApiController
{
    /// <summary>
    /// Retornar estabelecimentos próximos.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    [Route("estabelecimentos/{idLocalizacao}")]
    public async Task<IActionResult> ObterEstabelecimentos([FromRoute] Guid idLocalizacao)
    {
        try
        {
            return ResponseOk(await estabelecimentosClienteService.ObterEstabelecimentos(new UsuarioAutenticado(User), idLocalizacao));
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
    /// Retornar estabelecimentos próximos.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    [Route("estabelecimento/{idEstabelecimento}/produtos")]
    public async Task<IActionResult> ObterProdutosDoEstabelecimento([FromRoute] Guid idEstabelecimento)
    {
        try
        {
            return ResponseOk(await estabelecimentosClienteService.ObterProdutosDoEstabelecimento(new UsuarioAutenticado(User), idEstabelecimento));
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