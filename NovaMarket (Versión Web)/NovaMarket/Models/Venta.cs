using System.ComponentModel.DataAnnotations;

namespace NovaMarket.Models
{
    public class Venta
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public decimal Total { get; set; }

        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; }

        public int ClienteId { get; set; }

        public Cliente Cliente { get; set; }

        public List<DetalleVenta> Detalles { get; set; }
    }
}