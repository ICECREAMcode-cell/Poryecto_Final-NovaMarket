namespace NovahApp.Models
{
    public class Movimiento
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string TipoMovimiento { get; set; } = string.Empty; // 'ENTRADA', 'SALIDA'
        public int CantidadAnterior { get; set; }
        public int CantidadNueva { get; set; }
        public int UsuarioId { get; set; }
        public int ProductoId { get; set; }
    }
}