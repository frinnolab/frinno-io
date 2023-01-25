using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace frinno_api.ApiHelpers.AuthHelpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();

            if(allowAnonymous)
            return;

            var user = context.HttpContext.Items["User"];
            if(user == null)
            context.Result = new JsonResult(new {message = "Un uthorizedA"}){ StatusCode = StatusCodes.Status401Unauthorized};
        }
    }
}