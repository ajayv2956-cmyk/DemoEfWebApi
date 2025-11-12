using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using DemoEfWebApi.Services.Interfaces;

namespace DemoEfWebApi.Security
{
    public class TokenAuthenticationAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var request = actionContext.Request;
            if (!request.Headers.Contains("X-Auth-Token")) return false;

            var token = request.Headers.GetValues("X-Auth-Token").FirstOrDefault();
            if (string.IsNullOrWhiteSpace(token)) return false;

            // Resolve AuthService from dependency container or create new
            var authService = (IAuthService)actionContext.ControllerContext.Configuration.DependencyResolver.GetService(typeof(IAuthService));
            if (authService == null) return false;

            var valid = authService.ValidateTokenAsync(token).GetAwaiter().GetResult();
            return valid;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new { message = "Invalid or missing token" });
        }
    }
}