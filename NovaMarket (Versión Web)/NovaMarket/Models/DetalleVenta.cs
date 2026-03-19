using System.ComponentModel.DataAnnotations;

namespace NovaMarket.Models
{
    public class DetalleVenta
    {
        public int Id { get; set; }

        public int VentaId { get; set; }

        public Venta Venta { get; set; }

        public int ProductoId { get; set; }

        public Producto Producto { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal { get; set; }
    }
}