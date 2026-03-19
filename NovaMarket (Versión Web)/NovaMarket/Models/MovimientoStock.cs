using System.ComponentModel.DataAnnotations;

namespace NovaMarket.Models
{
    public class MovimientoStock
    {
        public int Id { get; set; }

        public int ProductoId { get; set; }

        public Producto Producto { get; set; }

        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; }

        public int CantidadAnterior { get; set; }

        public int CantidadNueva { get; set; }

        [Required]
        [StringLength(50)]
        public string TipoMovimiento { get; set; }

        public DateTime Fecha { get; set; }
    }
}