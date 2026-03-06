using System.ComponentModel.DataAnnotations;

namespace NovaMarket.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        public decimal PrecioCompra { get; set; }

        [Required]
        public decimal PrecioVenta { get; set; }

        [Required]
        public int Stock { get; set; }
    }
}