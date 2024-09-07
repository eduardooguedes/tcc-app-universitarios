using Dashdine.Application.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dashdine.Application.Filters
{
    public class ModeloDeValidacaoDeAtributo : IActionFilter
    {
        /// <inheritdoc/>
        public void OnActionExecuted(ActionExecutedContext actionContext)
        {
        }

        /// <inheritdoc/>
        public void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                string mensagemDeErro = actionContext.ModelState
                    .Values
                    .FirstOrDefault(e => e.ValidationState.Equals(ModelValidationState.Invalid))?
                    .Errors
                    .FirstOrDefault()?
                    .ErrorMessage ?? "";
                actionContext.Result = new BadRequestObjectResult(new RetornoPadrao(mensagemDeErro));
            }
        }
    }
}
