using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Dashdine.Application.Controllers.Shared
{
    [ApiController]
    public class ApiController : ControllerBase
    {
        protected IActionResult ResponseOk() =>
            Response(HttpStatusCode.OK);

        protected IActionResult ResponseOk(object result) =>
            Response(HttpStatusCode.OK, result);

        protected IActionResult ResponseCreated(object result) =>
            Response(HttpStatusCode.Created, result);
        protected IActionResult ResponseCreated() =>
            Response(HttpStatusCode.Created);

        protected IActionResult ResponseBadRequest(string erro) =>
            Response(HttpStatusCode.BadRequest, null, erro);
        protected IActionResult ResponseBadRequest() =>
            Response(HttpStatusCode.BadRequest, null, "Requisição inválida.");

        protected IActionResult ResponseNotFound(string erro) =>
            Response(HttpStatusCode.NotFound, null, erro);
        protected IActionResult ResponseNotFound() =>
            Response(HttpStatusCode.NotFound, null, "Recurso não encontrado.");

        protected IActionResult ResponseUnauthorized(string erro) =>
            Response(HttpStatusCode.Unauthorized, null, erro);
        protected IActionResult ResponseUnauthorized() =>
            Response(HttpStatusCode.Unauthorized, null, "Não autorizado.");

        protected IActionResult ResponseInternalServerError() =>
            Response(HttpStatusCode.InternalServerError);
        protected IActionResult ResponseInternalServerError(string erro) =>
            Response(HttpStatusCode.InternalServerError, null, erro);

        protected new ObjectResult Response(HttpStatusCode statusCode, object? data, string? erro) => StatusCode((int)statusCode, new RetornoPadrao(data, erro));

        protected new ObjectResult Response(HttpStatusCode statusCode, object data) =>
            Response(statusCode, data, null);

        protected new ObjectResult Response(HttpStatusCode statusCode, string mensagemDeErro) =>
            Response(statusCode, null, mensagemDeErro);

        protected new ObjectResult Response(HttpStatusCode statusCode) =>
            Response(statusCode, null, null);
    }
}
