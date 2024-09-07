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
[Authorize(Roles = Roles.Roles.RoleCliente)]
public class CartoesClienteController : ApiController
{
    private readonly ICartoesClienteService cartoesClienteService;

    public CartoesClienteController(ICartoesClienteService cartoesClienteService) => this.cartoesClienteService = cartoesClienteService;

    /// <summary>
    /// Retorna cartões do cliente.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    [Route("cartoes")]
    public async Task<IActionResult> ObterCartoes()
    {
        try
        {
            return ResponseOk(await cartoesClienteService.ObterLista(new UsuarioAutenticado(User)));
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
    /// Adiciona um cartão para o cliente. Retorna informações do cartão para listagem.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Route("cartao")]
    public async Task<IActionResult> AdicionarCartao(DtoDeCartaoDoCliente dto)
    {
        try
        {
            return ResponseOk(await cartoesClienteService.Adicionar(new UsuarioAutenticado(User), dto));
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
    /// Atualiza cartão do cliente.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPut]
    [Route("cartao/{idCartao}")]
    public async Task<IActionResult> AtualizarCartao([FromRoute] Guid idCartao, [FromBody] DtoDeCartaoDoClienteEdicao dto)
    {
        try
        {
            await cartoesClienteService.Atualizar(new UsuarioAutenticado(User), idCartao, dto);
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
    /// Exclui cartão do cliente.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpDelete]
    [Route("cartao/{idCartao}")]
    public async Task<IActionResult> ExcluirCartao([FromRoute] Guid idCartao)
    {
        try
        {
            await cartoesClienteService.Excluir(new UsuarioAutenticado(User), idCartao);
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