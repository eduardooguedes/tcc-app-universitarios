using Dashdine.Application.Attributes;
using Dashdine.Application.Controllers.Shared;
using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Service.Exceptions;
using Dashdine.Service.Interface.Estabelecimento;
using Dashdine.Service.Models.Estabelecimento;
using Dashdine.Service.Services.Estabelecimento;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Dashdine.Application.Controllers.Estabelecimento;

/// <summary>
/// Rota para estabelecimento.
/// </summary>
[Route("v1/estabelecimento")]
[Authorize]
public class EstabelecimentoController : ApiController
{
    private readonly IEstabelecimentoService estabelecimentoService;

    public EstabelecimentoController(
        IEstabelecimentoService estabelecimentoService
        )
    {
        this.estabelecimentoService = estabelecimentoService;
    }

    /// <summary>
    /// Cadastra um estabelecimento. Vincula usuário logado ao estabelecimento como administrador.
    /// </summary>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Route("")]
    [Authorize(Roles = Roles.Roles.RoleGestor)]
    public async Task<IActionResult> Cadastrar([FromBody] DtoDeEstabelecimento estabelecimento)
    {
        try
        {
            return ResponseOk(await estabelecimentoService.Cadastrar(new UsuarioAutenticado(User), estabelecimento));
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
    /// Atualiza a logo do estabelecimento.
    /// </summary>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Route("{idEstabelecimento}/logo")]
    [Authorize(Roles = Roles.Roles.RoleGestor)]
    public async Task<IActionResult> AtualizarLogo([FromRoute] Guid idEstabelecimento, [FromForm] IFormFile logo)
    {
        try
        {
            return ResponseOk(await estabelecimentoService.AtualizarLogo(new UsuarioAutenticado(User), idEstabelecimento, logo));
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
    /// Atualiza a situação do estabelecimento, entre 2 (ativo) e 3 (inativo).
    /// </summary>
    /// <param name="idEstabelecimento">Id do estabelecimento</param>
    /// <param name="novaSituacao">2 - Ativo | 3 - Inativo</param>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Authorize(Roles = Roles.Roles.RoleGestor)]
    [Route("{idEstabelecimento}/situacao/{novaSituacao}")]
    public async Task<IActionResult> AtualizarSituacao([FromRoute] Guid idEstabelecimento, [FromRoute] int novaSituacao)
    {
        try
        {
            await estabelecimentoService.AtualizarSituacao(new UsuarioAutenticado(User), idEstabelecimento, novaSituacao);
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
    /// Edita um estabelecimento.
    /// </summary>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPut]
    [Route("{idEstabelecimento}")]
    [Authorize(Roles = Roles.Roles.RoleGestor)]
    public async Task<IActionResult> Editar([FromRoute] Guid idEstabelecimento, [FromBody] DtoDeEstabelecimentoEdicao estabelecimento)
    {
        try
        {
            await estabelecimentoService.Editar(new UsuarioAutenticado(User), idEstabelecimento, estabelecimento);
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
    /// Edita um estabelecimento.
    /// </summary>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPut]
    [Route("{idEstabelecimento}/enderecoRetirada/{idEndereco}")]
    [Authorize(Roles = Roles.Roles.RoleGestor)]
    public async Task<IActionResult> Editar([FromRoute] Guid idEstabelecimento, [FromRoute] Guid idEndereco, [FromBody] DtoDeEnderecoRetirada endereco)
    {
        try
        {
            await estabelecimentoService.EditarEnderecoRetirada(new UsuarioAutenticado(User), idEstabelecimento, idEndereco, endereco);
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
    /// Retorna informações do estabelecimento.
    /// </summary>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> ObterEstabelecimento([FromRoute] Guid id)
    {
        try
        {
            UsuarioAutenticado gestorLogado = new(User);
            ProjecaoDeEstabelecimento estabelecimento = await estabelecimentoService.Obter(gestorLogado, id);
            return ResponseOk(estabelecimento);
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
    /// Retorna informações do estabelecimento.
    /// </summary>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    [Route("{id}/informacoes")]
    public async Task<IActionResult> ObterInformacoes([FromRoute] Guid id)
    {
        try
        {
            UsuarioAutenticado gestorLogado = new(User);
            ProjecaoDeInformacoesDoEstabelecimento informacoes = await estabelecimentoService.ObterInformacoes(gestorLogado, id);
            return ResponseOk(informacoes);
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
    /// Atualiza a logo do estabelecimento.
    /// </summary>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpDelete]
    [Route("{idEstabelecimento}/logo")]
    [Authorize(Roles = Roles.Roles.RoleGestor)]
    public async Task<IActionResult> RemoverLogo([FromRoute] Guid idEstabelecimento)
    {
        try
        {
            await estabelecimentoService.RemoverLogo(new UsuarioAutenticado(User), idEstabelecimento);
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
