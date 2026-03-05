namespace NovahApp.Models
{
    public static class UsuarioSesion
    {
        public static int Id { get; set; }
        public static string Nombre { get; set; } = string.Empty;
        public static int RolId { get; set; }
        public static string RolNombre { get; set; } = string.Empty;
    }
}