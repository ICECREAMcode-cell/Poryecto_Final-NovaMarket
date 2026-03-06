using System;
using Microsoft.Data.SqlClient;
using NovahApp.Models;

namespace NovahApp.Data
{
    public class InventarioRepository
    {
        public bool RegistrarEntradaStock(int productoId, int cantidadSumar)
        {
            using (SqlConnection conn = DbContext.Instance.GetConnection())
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    // 1. Obtener stock actual para el historial
                    string sqlActual = "SELECT stock FROM Producto WHERE id = @pid";
                    SqlCommand cmdA = new SqlCommand(sqlActual, conn, trans);
                    cmdA.Parameters.AddWithValue("@pid", productoId);
                    int stockAnterior = Convert.ToInt32(cmdA.ExecuteScalar());

                    // 2. Actualizar stock en la tabla Producto
                    string sqlUpdate = "UPDATE Producto SET stock = stock + @cant WHERE id = @pid";
                    SqlCommand cmdU = new SqlCommand(sqlUpdate, conn, trans);
                    cmdU.Parameters.AddWithValue("@cant", cantidadSumar);
                    cmdU.Parameters.AddWithValue("@pid", productoId);
                    cmdU.ExecuteNonQuery();

                    // 3. Registrar en MovimientoStock
                    string sqlMov = @"INSERT INTO MovimientoStock (tipoMovimiento, cantidadAnterior, cantidadNueva, usuario_id, producto_id) 
                                     VALUES ('ENTRADA', @ant, @nueva, @uid, @pid)";
                    SqlCommand cmdM = new SqlCommand(sqlMov, conn, trans);
                    cmdM.Parameters.AddWithValue("@ant", stockAnterior);
                    cmdM.Parameters.AddWithValue("@nueva", stockAnterior + cantidadSumar);
                    cmdM.Parameters.AddWithValue("@uid", UsuarioSesion.Id); // Quién hizo el cambio
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