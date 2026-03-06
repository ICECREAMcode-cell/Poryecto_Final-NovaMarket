using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovaMarket.Data;
using NovaMarket.Filters;

namespace NovaMarket.Controllers
{
    
    [SessionAuthorize(Rol = "Admin")]
    public class ReportesController : BaseController
    {
        private readonly NovaMarketContext _context;

        public ReportesController(NovaMarketContext context) : base(context)
        {
            _context = context;
        }

        // ======================================
        // REPORTE DE GANANCIAS MENSUALES
        // ======================================
        public async Task<IActionResult> GananciasMensuales(int? mes, int? anio)
        {
            mes ??= DateTime.Now.Month;
            anio ??= DateTime.Now.Year;

            var ingresos = await _context.Ventas
                .Where(v => v.Fecha.Month == mes && v.Fecha.Year == anio)
                .SumAsync(v => (decimal?)v.Total) ?? 0;

            var egresos = await _context.Egresos
                .Where(e => e.Fecha.Month == mes && e.Fecha.Year == anio)
                .SumAsync(e => (decimal?)e.Monto) ?? 0;

            var utilidad = ingresos - egresos;

            ViewBag.Mes = mes;
            ViewBag.Anio = anio;
            ViewBag.Ingresos = ingresos;
            ViewBag.Egresos = egresos;
            ViewBag.Utilidad = utilidad;

            return View();
        }

        // ======================================
        // PRODUCTOS MÁS VENDIDOS
        // ======================================
        public async Task<IActionResult> ProductosMasVendidos(DateTime? inicio, DateTime? fin)
        {
            inicio ??= DateTime.Now.AddMonths(-1);
            fin ??= DateTime.Now;

            var productos = await _context.DetalleVentas
                .Include(d => d.Producto)
                .Include(d => d.Venta)
                .Where(d => d.Venta.Fecha >= inicio && d.Venta.Fecha <= fin)
                .GroupBy(d => d.Producto.Nombre)
                .Select(g => new
                {
                    ProductoNombre = g.Key,
                    Cantidad = g.Sum(x => x.Cantidad),
                    Total = g.Sum(x => x.Subtotal)
                })
                .OrderByDescending(x => x.Cantidad)
                .ToListAsync();

            ViewBag.Inicio = inicio;
            ViewBag.Fin = fin;

            return View(productos);
        }

        // ======================================
        // VENTAS POR EMPLEADO
        // ======================================
        public async Task<IActionResult> VentasPorEmpleado()
        {
            var ventas = await _context.Ventas
                .Include(v => v.Usuario)
                .GroupBy(v => v.Usuario.Nombre)
                .Select(g => new
                {
                    Empleado = g.Key,
                    CantidadVentas = g.Count(),
                    TotalVendido = g.Sum(v => v.Total)
                })
                .OrderByDescending(x => x.TotalVendido)
                .ToListAsync();

            return View(ventas);
        }

        // ======================================
        // AUDITORÍA DEL SISTEMA
        // ======================================
        public async Task<IActionResult> Auditoria()
        {
            var auditorias = await _context.Auditorias
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();

            return View(auditorias);
        }
    }
}