using Dashdine.Application.Attributes;
using Dashdine.Application.Controllers.Shared;
using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Service.Exceptions;
using Dashdine.Service.Interface.Usuario;
using Dashdine.Service.Models.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashdine.Application.Controllers.Usuario;

/// <summary>
/// Rota para usuário.
/// </summary>
[Route("v1/usuario")]
[Authorize]
public class UsuarioController : ApiController
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    /// <summary>
    /// Rota criada para que usuário consiga criar uma solicitação de alteração de e-mail. Será gerado e enviado um código passa esse novo e-mail, que deve ser utilizado na rota POST /confirmarAtualizacaoEmail/{codigo}.
    /// </summary>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Route("email")]
    public async Task<IActionResult> AlterarEmail(DtoDeAtualizacaoDeEmail dto)
    {
        try
        {
            double tempoEmSegundosParaConfirmarCodigoGerado = await _usuarioService.SolicitarParaAtualizarEmail(new(User), dto.NovoEmail);
            return ResponseOk(tempoEmSegundosParaConfirmarCodigoGerado);
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
    /// Rota criada para que usuário possa confirmar, a partir de um código, a alteração de seu e-mail para o enviado anteriormente em POST /{idUsuario}/email/{novoEmail}.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Route("confirmarCodigoRecebidoEmail")]
    public async Task<IActionResult> ConfirmarAtualizacaoEmail([FromBody] DtoDeConfirmacaoDeEmail dto)
    {
        try
        {
            UsuarioAutenticado usuario = new(User);
            if (await _usuarioService.ConfirmarAtualizacaoDeEmail(usuario, dto.Codigo))
                return ResponseOk();

            return ResponseInternalServerError("Código incorreto.");
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
    /// Rota utilizada para atualizar senha do usuário. Deve ser consumida junto a um token que possua uma permissão extra de atualizar senha.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Route("recuperarSenha")]
    public async Task<IActionResult> RecuperarSenha([FromBody] DtoDeRecuperacaoDeSenha dto)
    {
        try
        {
            return ResponseOk(await _usuarioService.RecuperarSenha(new(User), dto));
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
    /// Rota utilizada para alterar senha do usuário. Recebe senha antiga e nova via body.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPut]
    [Route("senha")]
    public async Task<IActionResult> AlterarSenha([FromBody] DtoDeAlteracaoDeSenha dto)
    {
        try
        {
            await _usuarioService.AlterarSenha(new UsuarioAutenticado(User), dto);
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