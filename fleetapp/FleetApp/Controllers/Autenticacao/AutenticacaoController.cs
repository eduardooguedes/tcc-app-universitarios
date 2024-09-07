using Dashdine.Application.Attributes;
using Dashdine.Application.Controllers.Shared;
using Dashdine.CrossCutting.Enums.Usuario;
using Dashdine.Service.Exceptions;
using Dashdine.Service.Interface.Usuario;
using Dashdine.Service.Models.Cliente;
using Dashdine.Service.Models.Estabelecimento.Gestor;
using Dashdine.Service.Models.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashdine.Application.Controllers.Autenticacao;

/// <summary>
/// Rota para autenticação.
/// </summary>
[Route("v1/auth")]
public class AutenticacaoController : ApiController
{
    private readonly IAutenticacaoService _autenticacaoService;

    public AutenticacaoController(IAutenticacaoService autenticacaoService)
    {
        _autenticacaoService = autenticacaoService;
    }

    /// <summary>
    /// Autentica cliente no sistema, retornando token, informações do usuario e ultimo pedido, caso exista.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Route("login/cliente")]
    [AllowAnonymous]
    public async Task<IActionResult> AutenticarCliente([FromBody] DtoDeLogin login)
    {
        try
        {
            ProjecaoDeClienteLogado? clienteLogado = await _autenticacaoService.LoginCliente(login);
            if (clienteLogado != null)
                return ResponseOk(clienteLogado);

            return ResponseUnauthorized("Login ou senha incorretos.");
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
    /// Autentica gestor no sistema, retornando token e informações do usuario e estabelecimento.
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Route("login/gestor")]
    [AllowAnonymous]
    public async Task<IActionResult> AutenticarGestor([FromBody] DtoDeLogin login)
    {
        try
        {
            var gestorLogado = await _autenticacaoService.LoginGestor(login);
            if (gestorLogado is not null)
                return ResponseOk(gestorLogado);

            return ResponseUnauthorized("Login ou senha incorretos.");
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
    /// Solicita para recuperar senha do usuário a partir do e-mail.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="tipoDeUsuario">Tipos de usuário: Cliente, Gestor</param>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Route("solicitarParaRecuperarSenha/{email}/{tipoDeUsuario}")]
    [AllowAnonymous]
    public async Task<IActionResult> SolicitarParaRecuperarSenha([FromRoute] string email, [FromRoute] EnumTipoDeUsuario tipoDeUsuario)
    {
        try
        {
            double segundosParaExpirarRecuperacao = await _autenticacaoService.SolicitarParaRecuperarSenha(email, tipoDeUsuario);
            return ResponseOk(segundosParaExpirarRecuperacao);
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
    /// Autoriza recuperação de senha de usuário, caso código de confirmação esteja correto.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Route("autorizarRecuperacaoDeSenha/{tipoDeUsuario}")]
    [AllowAnonymous]
    public async Task<IActionResult> AutorizarRecuperacaoDeSenha([FromBody] DtoDeAutorizacaoDeRecuperacaoDeSenha dto, [FromRoute] EnumTipoDeUsuario tipoDeUsuario)
    {
        try
        {
            var usuarioLogado = await _autenticacaoService.AutorizarRecuperacaoDeSenha(dto.Email, dto.CodigoDeRecuperacao, tipoDeUsuario);
            return usuarioLogado == null ? ResponseInternalServerError("Código incorreto.") : ResponseOk(usuarioLogado);
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
