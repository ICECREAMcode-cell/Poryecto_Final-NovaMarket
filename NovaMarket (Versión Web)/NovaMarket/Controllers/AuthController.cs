using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovaMarket.Data;
using NovaMarket.Models;

namespace NovaMarket.Controllers
{
    public class AuthController : BaseController
    {
        private readonly NovaMarketContext _context;

        public AuthController(NovaMarketContext context) : base(context)
        {
            _context = context;
        }

        // GET: Auth/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (usuario == null)
            {
                ViewBag.Error = "Email o contraseña incorrectos";
                return View();
            }

            // Guardar datos en sesión
            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
            HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
            HttpContext.Session.SetString("Rol", usuario.Rol.Nombre);

            // Redirigir al inicio
            return RedirectToAction("Index", "Home");
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}