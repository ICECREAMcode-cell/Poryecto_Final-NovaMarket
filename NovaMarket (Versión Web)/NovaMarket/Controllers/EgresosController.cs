using NovaMarket.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovaMarket.Data;
using NovaMarket.Models;

namespace NovaMarket.Controllers
{
    [SessionAuthorize]
    public class EgresosController : BaseController
    {
        private readonly NovaMarketContext _context;

        public EgresosController(NovaMarketContext context) : base(context)
        {
            _context = context;
        }

        // GET: Egresos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Egresos.ToListAsync());
        }

        // GET: Egresos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Egresos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Egreso egreso)
        {
            if (ModelState.IsValid)
            {
                egreso.Fecha = DateTime.Now;

                _context.Egresos.Add(egreso);
                await _context.SaveChangesAsync();

                // AUDITORIA
                RegistrarAuditoria(
                    "Crear",
                    "Egresos",
                    egreso.Id,
                    $"Se registró un egreso por {egreso.Monto} - {egreso.Descripcion}"
                );

                return RedirectToAction(nameof(Index));
            }

            return View(egreso);
        }

        // GET: Egresos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var egreso = await _context.Egresos
                .FirstOrDefaultAsync(m => m.Id == id);

            if (egreso == null)
                return NotFound();

            return View(egreso);
        }

        // POST: Egresos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var egreso = await _context.Egresos.FindAsync(id);

            if (egreso != null)
            {
                _context.Egresos.Remove(egreso);
                await _context.SaveChangesAsync();

                // AUDITORIA
                RegistrarAuditoria(
                    "Eliminar",
                    "Egresos",
                    id,
                    $"Se eliminó un egreso con ID {id}"
                );
            }

            return RedirectToAction(nameof(Index));
        }
    }
}