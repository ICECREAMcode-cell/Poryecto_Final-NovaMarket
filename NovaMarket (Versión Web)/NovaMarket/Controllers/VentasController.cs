using NovaMarket.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NovaMarket.Data;
using NovaMarket.Models;

namespace NovaMarket.Controllers
{
    [SessionAuthorize]
    public class VentasController : BaseController
    {
        private readonly NovaMarketContext _context;

        public VentasController(NovaMarketContext context) : base(context)
        {
            _context = context;
        }

        // =========================
        // LISTA DE VENTAS
        // =========================
        public async Task<IActionResult> Index()
        {
            var ventas = _context.Ventas
                .Include(v => v.Usuario)
                .Include(v => v.Cliente);

            return View(await ventas.ToListAsync());
        }

        // =========================
        // DETALLE DE VENTA
        // =========================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var venta = await _context.Ventas
                .Include(v => v.Usuario)
                .Include(v => v.Cliente)
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null)
                return NotFound();

            return View(venta);
        }

        // =========================
        // FORMULARIO DE VENTA
        // =========================
        public IActionResult Create()
        {
            ViewBag.Productos = new SelectList(_context.Productos, "Id", "Nombre");
            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nombre");
            return View();
        }

        // =========================
        // CREAR VENTA
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int clienteId, List<int> productoId, List<int> cantidad)
        {
            if (productoId == null || cantidad == null || productoId.Count == 0)
            {
                ViewBag.Error = "Debe seleccionar productos.";
                ViewBag.Productos = new SelectList(_context.Productos, "Id", "Nombre");
                ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nombre");
                return View();
            }

            int usuarioId = HttpContext.Session.GetInt32("UsuarioId") ?? 0;
            string usuarioNombre = HttpContext.Session.GetString("UsuarioNombre") ?? "Sistema";

            // =========================
            // VALIDAR STOCK
            // =========================
            for (int i = 0; i < productoId.Count; i++)
            {
                var producto = await _context.Productos.FindAsync(productoId[i]);

                if (producto == null)
                    continue;

                if (producto.Stock < cantidad[i])
                {
                    ViewBag.Error = $"No hay suficiente stock para {producto.Nombre}. Disponible: {producto.Stock}";
                    ViewBag.Productos = new SelectList(_context.Productos, "Id", "Nombre");
                    ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nombre");
                    return View();
                }
            }

            // =========================
            // CREAR VENTA
            // =========================
            var venta = new Venta
            {
                Fecha = DateTime.Now,
                UsuarioId = usuarioId,
                ClienteId = clienteId,
                Total = 0
            };

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            decimal totalVenta = 0;

            // =========================
            // REGISTRAR DETALLES
            // =========================
            for (int i = 0; i < productoId.Count; i++)
            {
                var producto = await _context.Productos.FindAsync(productoId[i]);

                if (producto == null)
                    continue;

                int stockAnterior = producto.Stock;

                producto.Stock -= cantidad[i];

                decimal subtotal = producto.PrecioVenta * cantidad[i];

                var detalle = new DetalleVenta
                {
                    VentaId = venta.Id,
                    ProductoId = producto.Id,
                    Cantidad = cantidad[i],
                    PrecioUnitario = producto.PrecioVenta,
                    Subtotal = subtotal
                };

                _context.DetalleVentas.Add(detalle);

                totalVenta += subtotal;

                var movimiento = new MovimientoStock
                {
                    ProductoId = producto.Id,
                    UsuarioId = usuarioId,
                    CantidadAnterior = stockAnterior,
                    CantidadNueva = producto.Stock,
                    TipoMovimiento = "Venta",
                    Fecha = DateTime.Now
                };

                _context.MovimientosStock.Add(movimiento);
            }

            venta.Total = totalVenta;

            await _context.SaveChangesAsync();

            // =========================
            // AUDITORIA
            // =========================
            var auditoria = new Auditoria
            {
                UsuarioId = usuarioId,
                UsuarioNombre = usuarioNombre,
                Accion = "Registrar Venta",
                Tabla = "Ventas",
                RegistroId = venta.Id,
                Fecha = DateTime.Now,
                Descripcion = $"Se registró la venta #{venta.Id} por {venta.Total}"
            };

            _context.Auditorias.Add(auditoria);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // CONFIRMAR ELIMINACION
        // =========================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var venta = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (venta == null)
                return NotFound();

            return View(venta);
        }

        // =========================
        // ELIMINAR VENTA
        // =========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.Detalles)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null)
                return NotFound();

            int usuarioId = HttpContext.Session.GetInt32("UsuarioId") ?? 0;
            string usuarioNombre = HttpContext.Session.GetString("UsuarioNombre") ?? "Sistema";

            // =========================
            // DEVOLVER STOCK
            // =========================
            foreach (var detalle in venta.Detalles)
            {
                var producto = await _context.Productos.FindAsync(detalle.ProductoId);

                if (producto != null)
                {
                    int stockAnterior = producto.Stock;

                    producto.Stock += detalle.Cantidad;

                    var movimiento = new MovimientoStock
                    {
                        ProductoId = producto.Id,
                        UsuarioId = usuarioId,
                        CantidadAnterior = stockAnterior,
                        CantidadNueva = producto.Stock,
                        TipoMovimiento = "Cancelación Venta",
                        Fecha = DateTime.Now
                    };

                    _context.MovimientosStock.Add(movimiento);
                }
            }

            // ELIMINAR DETALLES
            if (venta.Detalles != null && venta.Detalles.Any())
                _context.DetalleVentas.RemoveRange(venta.Detalles);

            // ELIMINAR VENTA
            _context.Ventas.Remove(venta);

            await _context.SaveChangesAsync();

            // =========================
            // AUDITORIA
            // =========================
            var auditoria = new Auditoria
            {
                UsuarioId = usuarioId,
                UsuarioNombre = usuarioNombre,
                Accion = "Eliminar Venta",
                Tabla = "Ventas",
                RegistroId = id,
                Fecha = DateTime.Now,
                Descripcion = $"Se eliminó la venta #{id}"
            };

            _context.Auditorias.Add(auditoria);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}