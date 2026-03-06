using System.ComponentModel.DataAnnotations;

namespace NovaMarket.Models
{
    public class Egreso
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Descripcion { get; set; }

        [Required]
        public decimal Monto { get; set; }

        public DateTime Fecha { get; set; }
    }
}