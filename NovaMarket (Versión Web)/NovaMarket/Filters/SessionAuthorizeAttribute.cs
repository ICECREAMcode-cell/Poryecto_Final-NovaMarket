using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NovaMarket.Filters
{
    public class SessionAuthorize : ActionFilterAttribute
    {
        public string Rol { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Verificar si existe sesión de usuario
            var usuarioId = context.HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }

            // Verificar rol si el controlador lo requiere
            if (!string.IsNullOrEmpty(Rol))
            {
                var rolUsuario = context.HttpContext.Session.GetString("Rol");

                if (rolUsuario == null || rolUsuario != Rol)
                {
                    context.Result = new RedirectToActionResult("Index", "Home", null);
                    return;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}