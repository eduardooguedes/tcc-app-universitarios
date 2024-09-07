using Dashdine.Application.Attributes;
using Dashdine.Application.Controllers.Shared;
using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Service.Exceptions;
using Dashdine.Service.Interface.Cliente;
using Dashdine.Service.Models.Cliente;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashdine.Application.Controllers.Cliente;

[Route("v1/cliente")]
public class ClienteController : ApiController
{
    private readonly IClienteService clienteService;

    public ClienteController(IClienteService clienteService)
    {
        this.clienteService = clienteService;
    }

    /// <summary>
    /// Cadastra um cliente. Retorna token.
    /// </summary>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Route("")]
    [AllowAnonymous]
    public async Task<IActionResult> Novo([FromBody] DtoDeCliente cliente)
    {
        try
        {
            return ResponseOk(await clienteService.Novo(cliente));
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
    /// Edita um cliente. Retorna um token.
    /// </summary>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPut]
    [Route("")]
    [Authorize(Roles = Roles.Roles.RoleCliente)]
    public async Task<IActionResult> Editar([FromBody] DtoDeCliente cliente)
    {
        try
        {
            await clienteService.Editar(new UsuarioAutenticado(User).Id, cliente);
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
