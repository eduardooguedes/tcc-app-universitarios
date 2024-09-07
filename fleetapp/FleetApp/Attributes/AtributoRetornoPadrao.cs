using Dashdine.Application.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Dashdine.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class AtributoRetornoPadrao : ProducesResponseTypeAttribute
    {
        public AtributoRetornoPadrao(int statusCode) : base(typeof(RetornoPadrao), statusCode) { }
    }
}
