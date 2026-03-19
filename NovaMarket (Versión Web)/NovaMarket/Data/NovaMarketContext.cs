using Microsoft.EntityFrameworkCore;
using NovaMarket.Models;

namespace NovaMarket.Data
{
    public class NovaMarketContext : DbContext
    {
        public NovaMarketContext(DbContextOptions<NovaMarketContext> options)
            : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }
        public DbSet<Egreso> Egresos { get; set; }
        public DbSet<MovimientoStock> MovimientosStock { get; set; }
        public DbSet<Auditoria> Auditorias { get; set; }
    }
}