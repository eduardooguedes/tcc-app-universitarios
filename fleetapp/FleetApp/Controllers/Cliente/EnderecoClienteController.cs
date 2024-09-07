using Dashdine.Application.Attributes;
using Dashdine.Application.Controllers.Shared;
using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Service.Exceptions;
using Dashdine.Service.Interface.Cliente;
using Dashdine.Service.Models.Cliente;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashdine.Application.Controllers.Cliente;

/// <summary>
/// Rota para endereço de cobrança do cliente.
/// </summary>
[Route("v1/cliente")]
[Authorize(Roles = Roles.Roles.RoleCliente)]
public class EnderecoClienteController : ApiController
{
    private readonly IEnderecoClienteService _enderecoClienteService;
    public EnderecoClienteController(IEnderecoClienteService enderecoCobrancaService)
    {
        _enderecoClienteService = enderecoCobrancaService;
    }

    /// <summary>
    /// Adiciona endereço ou localização do cliente.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Route("endereco")]
    public async Task<IActionResult> AdicionarEndereco([FromBody] DtoDeEnderecoCliente dto)
    {
        try
        {
            return ResponseCreated(await _enderecoClienteService.Adicionar(new UsuarioAutenticado(User), dto));
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
    /// Atualiza endereço ou localização do cliente.
    /// </summary>
    /// <param name="idEndereco"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPut]
    [Route("endereco/{idEndereco}")]
    public async Task<IActionResult> AtualizarEndereco([FromRoute] Guid idEndereco, [FromBody] DtoDeEnderecoCliente dto)
    {
        try
        {
            await _enderecoClienteService.Atualizar(new UsuarioAutenticado(User), idEndereco, dto);
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
    /// Remove endereço ou localização do cliente.
    /// </summary>
    /// <param name="idEndereco"></param>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpDelete]
    [Route("endereco/{idEndereco}")]
    public async Task<IActionResult> RemoverEndereco([FromRoute] Guid idEndereco)
    {
        try
        {
            await _enderecoClienteService.Remover(new UsuarioAutenticado(User), idEndereco);
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
    /// 1 - "minha localização" ou retirada | 2 - pagamento
    /// </summary>
    /// <param name="idTipoEndereco"></param>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    [Route("enderecos")]
    public async Task<IActionResult> ObterEnderecos(int? idTipoEndereco)
    {
        try
        {
            return ResponseOk(await _enderecoClienteService.ObterLista(new UsuarioAutenticado(User), idTipoEndereco));
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
