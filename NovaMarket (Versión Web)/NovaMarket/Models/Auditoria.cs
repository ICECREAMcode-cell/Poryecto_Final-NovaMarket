using System;
using System.ComponentModel.DataAnnotations;

namespace NovaMarket.Models
{
    public class Auditoria
    {
        public int Id { get; set; }

        [Required]
        public string Accion { get; set; }

        [Required]
        public string Tabla { get; set; }

        public int? RegistroId { get; set; }

        public string Descripcion { get; set; }

        public int UsuarioId { get; set; }

        public string UsuarioNombre { get; set; }

        public DateTime Fecha { get; set; }
    }
}