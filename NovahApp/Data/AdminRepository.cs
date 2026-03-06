using System.Data;
using Microsoft.Data.SqlClient;

namespace NovahApp.Data
{
    public class AdminRepository
    {
        private string connectionString = "Server=localhost;Database=Nova_DB;Integrated Security=True;Encrypt=False";

        public DataTable ObtenerReporteFinanciero()
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT p.nombre AS Producto, 
                           SUM(dv.cantidad) AS Vendidos,
                           SUM(dv.cantidad * p.precioCompra) AS Inversion,
                           SUM(dv.cantidad * dv.precioUnitario) AS Ingresos,
                           SUM(dv.cantidad * (dv.precioUnitario - p.precioCompra)) AS UtilidadNeta
                           FROM DetalleVenta dv
                           JOIN Producto p ON dv.producto_id = p.id
                           GROUP BY p.nombre";
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(dt);
            }
            return dt;
        }
    }
}