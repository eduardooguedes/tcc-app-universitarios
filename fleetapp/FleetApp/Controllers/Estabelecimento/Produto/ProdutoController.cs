using Dashdine.Application.Attributes;
using Dashdine.Application.Controllers.Shared;
using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Service.Exceptions;
using Dashdine.Service.Interface.Produto;
using Dashdine.Service.Models.Produto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashdine.Application.Controllers.Produto;

/// <summary>
/// Rota para produto.
/// </summary>
[Route("v1/estabelecimento/produto")]
[Authorize]
public class ProdutoController(
    IProdutoService produtoService) : ApiController
{
    /// <summary>
    /// Cadastra um produto no estabelecimento do gestor logado.
    /// </summary>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Route("")]
    [Authorize(Roles = Roles.Roles.RoleGestor)]
    public async Task<IActionResult> Cadastrar([FromBody] DtoDeProduto produto)
    {
        try
        {
            var projecaoProduto = await produtoService.Cadastrar(new UsuarioAutenticado(User), produto);
            return ResponseOk(projecaoProduto);
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
    /// Cadastra um produto no estabelecimento do gestor logado.
    /// </summary>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Route("{idProduto}/imagem")]
    [Authorize(Roles = Roles.Roles.RoleGestor)]
    public async Task<IActionResult> AtualizarImagem([FromRoute] Guid idProduto, [FromForm] IFormFile imagem)
    {
        try
        {
            string caminhoImagem = await produtoService.AtualizarImagem(new UsuarioAutenticado(User), idProduto, imagem);
            return ResponseOk(caminhoImagem);
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
    /// Edita um produto no estabelecimento do gestor logado.
    /// </summary>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPut]
    [Route("{idProduto}")]
    [Authorize(Roles = Roles.Roles.RoleGestor)]
    public async Task<IActionResult> Atualizar([FromRoute] Guid idProduto, [FromBody] DtoDeProduto produto)
    {
        try
        {
            var projecaoProduto = await produtoService.Atualizar(new UsuarioAutenticado(User), idProduto, produto);
            return ResponseOk(projecaoProduto);
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
    /// Edita a situação de um produto do estabelecimento do gestor logado.
    /// </summary>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpPut]
    [Route("{idProduto}/situacao/{idNovaSituacao}")]
    [Authorize(Roles = Roles.Roles.RoleGestor)]
    public async Task<IActionResult> AtualizarSituacao([FromRoute] Guid idProduto, [FromRoute] int idNovaSituacao)
    {
        try
        {
            await produtoService.AtualizarSituacao(new UsuarioAutenticado(User), idProduto, idNovaSituacao);
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
    /// Retorna um produto.
    /// </summary>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    [Route("{idProduto}")]
    [Authorize(Roles = Roles.Roles.RoleGestor)]
    public async Task<IActionResult> Obter([FromRoute] Guid idProduto)
    {
        try
        {
            ProjecaoDeProduto projecao = await produtoService.ObterPorId(new UsuarioAutenticado(User), idProduto);
            return ResponseOk(projecao);
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
    /// Retorna todos os produtos.
    /// </summary>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    [Route("lista")]
    [Authorize(Roles = Roles.Roles.RoleGestor)]
    //[ResponseCache(CacheProfileName = "default", Duration = 60 * 20)]
    public async Task<IActionResult> ObterTodos([FromQuery] int? categoria)
    {
        try
        {
            return ResponseOk(await produtoService.GestorObterTodos(new UsuarioAutenticado(User), categoria));
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
    /// Remove imagem do produto.
    /// </summary>
    /// <returns></returns>
    [AtributoRetornoPadrao(StatusCodes.Status200OK)]
    [AtributoRetornoPadrao(StatusCodes.Status401Unauthorized)]
    [HttpDelete]
    [Route("{idProduto}/imagem")]
    [Authorize(Roles = Roles.Roles.RoleGestor)]
    public async Task<IActionResult> RemoverImagem([FromRoute] Guid idProduto)
    {
        try
        {
            await produtoService.RemoverImagem(new UsuarioAutenticado(User), idProduto);
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
