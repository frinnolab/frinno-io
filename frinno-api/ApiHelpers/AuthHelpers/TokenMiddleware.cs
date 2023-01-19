using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Authentication;

namespace frinno_api.ApiHelpers.AuthHelpers
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate requestDelegate;

        public TokenMiddleware(RequestDelegate rDelegate)
        {
            requestDelegate = rDelegate;
        }
        async Task Invoke(HttpContext context, IAuthService authService, ITokenService tokenService)
        {
            var token = context.Request.Headers["Authorization"]
            .FirstOrDefault()?.Split(" ").Last();

            var userId = tokenService.Validate(token??"");

            if(userId!=null)
            {
                context.Items["User"] = authService.FindUserById(userId.Value);
            }

            await requestDelegate(context);

        }
    }
}