using Hackathon_segunda_chamada.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Hackathon_segunda_chamada.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AutorizarPerfilAttribute : ActionFilterAttribute
    {
        private readonly PerfilUsuario[] _perfisPermitidos;

        public AutorizarPerfilAttribute(params PerfilUsuario[] perfis)
        {
            _perfisPermitidos = perfis;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;

            // Sem autenticação → redireciona para login
            if (user?.Identity == null || !user.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            // Lê o perfil do claim Role
            var perfilStr = user.FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrEmpty(perfilStr) ||
                !Enum.TryParse<PerfilUsuario>(perfilStr, out var perfil) ||
                !_perfisPermitidos.Contains(perfil))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
