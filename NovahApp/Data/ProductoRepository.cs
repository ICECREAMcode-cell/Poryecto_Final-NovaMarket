using System;
using Microsoft.Data.SqlClient;
using NovahApp.Models;

namespace NovahApp.Data
{
    public class ProductoRepository
    {
        public bool RegistrarNuevoProducto(string nombre, double pCompra, double pVenta, int stock, string imagen)
        {
            using (SqlConnection conn = DbContext.Instance.GetConnection())
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    // 1. Insertar el Producto
                    string sqlProd = @"INSERT INTO Producto (nombre, precioCompra, precioVenta, stock, imagen_ruta) 
                                      OUTPUT INSERTED.id 
                                      VALUES (@nom, @pc, @pv, @st, @img)";
                    
                    SqlCommand cmdP = new SqlCommand(sqlProd, conn, trans);
                    cmdP.Parameters.AddWithValue("@nom", nombre);
                    cmdP.Parameters.AddWithValue("@pc", pCompra);
                    cmdP.Parameters.AddWithValue("@pv", pVenta);
                    cmdP.Parameters.AddWithValue("@st", stock);
                    cmdP.Parameters.AddWithValue("@img", imagen);
                    
                    int newId = (int)cmdP.ExecuteScalar();

                    // 2. Registrar el movimiento inicial en MovimientoStock
                    string sqlMov = @"INSERT INTO MovimientoStock (tipoMovimiento, cantidadAnterior, cantidadNueva, usuario_id, producto_id) 
                                     VALUES ('ENTRADA', 0, @st, @uid, @pid)";
                    
                    SqlCommand cmdM = new SqlCommand(sqlMov, conn, trans);
                    cmdM.Parameters.AddWithValue("@st", stock);
                    cmdM.Parameters.AddWithValue("@uid", UsuarioSesion.Id);
                    cmdM.Parameters.AddWithValue("@pid", newId);
                    cmdM.ExecuteNonQuery();

                    trans.Commit();
                    return true;
                }
                catch { trans.Rollback(); return false; }
            }
        }
    }
}