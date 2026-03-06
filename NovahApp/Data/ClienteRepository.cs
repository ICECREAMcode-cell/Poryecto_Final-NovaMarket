using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace NovahApp.Data
{
    public class ClienteRepository
    {
        private string connectionString = "Server=localhost;Database=Nova_DB;Integrated Security=True;Encrypt=False";

        public DataTable ObtenerCatalogoPublico()
        {
            DataTable dt = new DataTable();
            // El cliente no debe ver el precioCompra por seguridad del negocio
            string sql = "SELECT id, nombre, precioVenta, stock, ImagenPath FROM Producto WHERE stock > 0";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable ObtenerMisCompras(int clienteId)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT id, fecha, total FROM Venta WHERE cliente_id = @cid ORDER BY fecha DESC";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@cid", clienteId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }
    }
}