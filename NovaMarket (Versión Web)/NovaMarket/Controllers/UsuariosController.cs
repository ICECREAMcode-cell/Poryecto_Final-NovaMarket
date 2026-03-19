using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NovaMarket.Data;
using NovaMarket.Models;
using NovaMarket.Filters;

namespace NovaMarket.Controllers
{
    [SessionAuthorize(Rol = "Admin")]
    public class UsuariosController : BaseController
    {
        private readonly NovaMarketContext _context;

        public UsuariosController(NovaMarketContext context) : base(context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var usuarios = _context.Usuarios
                .Include(u => u.Rol);

            return View(await usuarios.ToListAsync());
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(_context.Roles, "Id", "Nombre");
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                int usuarioId = HttpContext.Session.GetInt32("UsuarioId") ?? 0;
                string usuarioNombre = HttpContext.Session.GetString("UsuarioNombre") ?? "Sistema";

                // AUDITORIA
                var auditoria = new Auditoria
                {
                    UsuarioId = usuarioId,
                    UsuarioNombre = usuarioNombre,
                    Accion = "Crear Usuario",
                    Tabla = "Usuarios",
                    RegistroId = usuario.Id,
                    Fecha = DateTime.Now,
                    Descripcion = $"Se creó el usuario {usuario.Nombre} ({usuario.Email})"
                };

                _context.Auditorias.Add(auditoria);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Roles = new SelectList(_context.Roles, "Id", "Nombre", usuario.RolId);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound();

            ViewBag.Roles = new SelectList(_context.Roles, "Id", "Nombre", usuario.RolId);

            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Usuario usuario)
        {
            if (id != usuario.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Usuarios.Update(usuario);
                    await _context.SaveChangesAsync();

                    int usuarioId = HttpContext.Session.GetInt32("UsuarioId") ?? 0;
                    string usuarioNombre = HttpContext.Session.GetString("UsuarioNombre") ?? "Sistema";

                    // AUDITORIA
                    var auditoria = new Auditoria
                    {
                        UsuarioId = usuarioId,
                        UsuarioNombre = usuarioNombre,
                        Accion = "Editar Usuario",
                        Tabla = "Usuarios",
                        RegistroId = usuario.Id,
                        Fecha = DateTime.Now,
                        Descripcion = $"Se editó el usuario {usuario.Nombre}"
                    };

                    _context.Auditorias.Add(auditoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Roles = new SelectList(_context.Roles, "Id", "Nombre", usuario.RolId);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();

                int usuarioId = HttpContext.Session.GetInt32("UsuarioId") ?? 0;
                string usuarioNombre = HttpContext.Session.GetString("UsuarioNombre") ?? "Sistema";

                // AUDITORIA
                var auditoria = new Auditoria
                {
                    UsuarioId = usuarioId,
                    UsuarioNombre = usuarioNombre,
                    Accion = "Eliminar Usuario",
                    Tabla = "Usuarios",
                    RegistroId = usuario.Id,
                    Fecha = DateTime.Now,
                    Descripcion = $"Se eliminó el usuario {usuario.Nombre}"
                };

                _context.Auditorias.Add(auditoria);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}