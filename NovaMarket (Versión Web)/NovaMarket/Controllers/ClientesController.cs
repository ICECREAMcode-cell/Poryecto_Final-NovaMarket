using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovaMarket.Data;
using NovaMarket.Models;

namespace NovaMarket.Controllers
{
    public class ClientesController : BaseController
    {
        private readonly NovaMarketContext _context;

        public ClientesController(NovaMarketContext context) : base(context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clientes.ToListAsync());
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                // Auditoría
                RegistrarAuditoria(
                    "Crear",
                    "Clientes",
                    cliente.Id,
                    $"Se creó el cliente {cliente.Nombre}"
                );

                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (id != cliente.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();

                    // Auditoría
                    RegistrarAuditoria(
                        "Editar",
                        "Clientes",
                        cliente.Id,
                        $"Se editó el cliente {cliente.Nombre}"
                    );
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Clientes.Any(e => e.Id == cliente.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();

                // Auditoría
                RegistrarAuditoria(
                    "Eliminar",
                    "Clientes",
                    cliente.Id,
                    $"Se eliminó el cliente {cliente.Nombre}"
                );
            }

            return RedirectToAction(nameof(Index));
        }
    }
}