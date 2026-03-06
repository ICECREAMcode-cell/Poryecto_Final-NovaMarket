using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace NovahApp.Data
{
    public class EmpleadoRepository
    {
        private string connectionString = "Server=localhost;Database=Nova_DB;Integrated Security=True;Encrypt=False";

        public DataTable ObtenerInventarioCompleto()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT id, nombre, precioVenta, stock FROM Producto";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(dt);
            }
            return dt;
        }

        public bool RegistrarEntradaStock(int productoId, int cantidad, int usuarioId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    // 1. Actualizar Stock
                    string sqlUpdate = "UPDATE Producto SET stock = stock + @q WHERE id = @pid";
                    SqlCommand cmdU = new SqlCommand(sqlUpdate, conn, trans);
                    cmdU.Parameters.AddWithValue("@q", cantidad);
                    cmdU.Parameters.AddWithValue("@pid", productoId);
                    cmdU.ExecuteNonQuery();

                    // 2. Registrar Movimiento de Auditoría
                    string sqlMov = @"INSERT INTO MovimientoStock (tipoMovimiento, cantidadAnterior, cantidadNueva, usuario_id, producto_id)
                                     SELECT 'ENTRADA', stock - @q, stock, @uid, @pid 
                                     FROM Producto WHERE id = @pid";
                    SqlCommand cmdM = new SqlCommand(sqlMov, conn, trans);
                    cmdM.Parameters.AddWithValue("@q", cantidad);
                    cmdM.Parameters.AddWithValue("@uid", usuarioId);
                    cmdM.Parameters.AddWithValue("@pid", productoId);
                    cmdM.ExecuteNonQuery();

                    trans.Commit();
                    return true;
                }
                catch { trans.Rollback(); return false; }
            }
        }
    }
}