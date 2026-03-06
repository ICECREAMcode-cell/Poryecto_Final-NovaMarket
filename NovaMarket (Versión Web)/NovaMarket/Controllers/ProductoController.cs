using NovaMarket.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovaMarket.Data;
using NovaMarket.Models;

namespace NovaMarket.Controllers
{
    [SessionAuthorize]
    public class ProductosController : BaseController
    {
        private readonly NovaMarketContext _context;

        public ProductosController(NovaMarketContext context) : base(context)
        {
            _context = context;
        }

        // GET: Productos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Productos.ToListAsync());
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.Id == id);

            if (producto == null)
                return NotFound();

            return View(producto);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Productos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                var usuarioId = HttpContext.Session.GetInt32("UsuarioId") ?? 0;
                var usuarioNombre = HttpContext.Session.GetString("UsuarioNombre") ?? "Sistema";

                // Guardar primero el producto
                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();

                // Movimiento de Stock
                var movimiento = new MovimientoStock
                {
                    ProductoId = producto.Id,
                    UsuarioId = usuarioId,
                    CantidadAnterior = 0,
                    CantidadNueva = producto.Stock,
                    TipoMovimiento = "Ingreso Inicial",
                    Fecha = DateTime.Now
                };

                _context.MovimientosStock.Add(movimiento);

                // Auditoría
                var auditoria = new Auditoria
                {
                    UsuarioId = usuarioId,
                    UsuarioNombre = usuarioNombre,
                    Accion = "Crear Producto",
                    Tabla = "Productos",
                    RegistroId = producto.Id,
                    Fecha = DateTime.Now,
                    Descripcion = $"Se creó el producto {producto.Nombre} con stock {producto.Stock}"
                };

                _context.Auditorias.Add(auditoria);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(producto);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return NotFound();

            return View(producto);
        }

        // POST: Productos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Producto producto)
        {
            if (id != producto.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var productoBD = await _context.Productos
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.Id == id);

                    int stockAnterior = productoBD.Stock;

                    _context.Productos.Update(producto);

                    var usuarioId = HttpContext.Session.GetInt32("UsuarioId") ?? 0;
                    var usuarioNombre = HttpContext.Session.GetString("UsuarioNombre") ?? "Sistema";

                    // Registrar movimiento si cambia el stock
                    if (stockAnterior != producto.Stock)
                    {
                        var movimiento = new MovimientoStock
                        {
                            ProductoId = producto.Id,
                            UsuarioId = usuarioId,
                            CantidadAnterior = stockAnterior,
                            CantidadNueva = producto.Stock,
                            TipoMovimiento = "Ajuste de Stock",
                            Fecha = DateTime.Now
                        };

                        _context.MovimientosStock.Add(movimiento);
                    }

                    // Auditoría
                    var auditoria = new Auditoria
                    {
                        UsuarioId = usuarioId,
                        UsuarioNombre = usuarioNombre,
                        Accion = "Editar Producto",
                        Tabla = "Productos",
                        RegistroId = producto.Id,
                        Fecha = DateTime.Now,
                        Descripcion = $"Se editó el producto {producto.Nombre}"
                    };

                    _context.Auditorias.Add(auditoria);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Productos.Any(e => e.Id == producto.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(producto);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.Id == id);

            if (producto == null)
                return NotFound();

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto != null)
            {
                var usuarioId = HttpContext.Session.GetInt32("UsuarioId") ?? 0;
                var usuarioNombre = HttpContext.Session.GetString("UsuarioNombre") ?? "Sistema";

                // Movimiento de Stock
                var movimiento = new MovimientoStock
                {
                    ProductoId = producto.Id,
                    UsuarioId = usuarioId,
                    CantidadAnterior = producto.Stock,
                    CantidadNueva = 0,
                    TipoMovimiento = "Eliminación de Producto",
                    Fecha = DateTime.Now
                };

                _context.MovimientosStock.Add(movimiento);

                // Auditoría
                var auditoria = new Auditoria
                {
                    UsuarioId = usuarioId,
                    UsuarioNombre = usuarioNombre,
                    Accion = "Eliminar Producto",
                    Tabla = "Productos",
                    RegistroId = producto.Id,
                    Fecha = DateTime.Now,
                    Descripcion = $"Se eliminó el producto {producto.Nombre}"
                };

                _context.Auditorias.Add(auditoria);

                _context.Productos.Remove(producto);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}