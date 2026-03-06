using Microsoft.AspNetCore.Mvc;
using NovaMarket.Data;
using NovaMarket.Models;

namespace NovaMarket.Controllers
{
    public class BaseController : Controller
    {
        protected readonly NovaMarketContext _context;

        public BaseController(NovaMarketContext context)
        {
            _context = context;
        }

        protected void RegistrarAuditoria(string accion, string tabla, int? registroId, string descripcion)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId") ?? 0;
            var usuarioNombre = HttpContext.Session.GetString("UsuarioNombre");

            // Evitar que UsuarioNombre sea NULL
            if (string.IsNullOrEmpty(usuarioNombre))
            {
                usuarioNombre = "Sistema";
            }

            var auditoria = new Auditoria
            {
                Accion = accion,
                Tabla = tabla,
                RegistroId = registroId,
                Descripcion = descripcion,
                UsuarioId = usuarioId,
                UsuarioNombre = usuarioNombre,
                Fecha = DateTime.Now
            };

            _context.Auditorias.Add(auditoria);
            _context.SaveChanges();
        }
    }
}