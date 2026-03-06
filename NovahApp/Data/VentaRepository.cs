using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using NovahApp.Models;

namespace NovahApp.Data
{
    // Clase CarritoItem con el CONSTRUCTOR que faltaba
    public class CarritoItem
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public double PrecioVenta { get; set; }

        public CarritoItem(int id, string nom, int cant, double precio)
        {
            ProductoId = id;
            Nombre = nom;
            Cantidad = cant;
            PrecioVenta = precio;
        }
    }

    public class VentaRepository
    {
        public bool ProcesarCompra(int usuarioId, List<CarritoItem> items, double total)
        {
            using (SqlConnection conn = DbContext.Instance.GetConnection())
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    // 1. Insertar Venta (Unificado a tabla Usuario)
                    string sqlVenta = @"INSERT INTO Venta (fecha, total, empleado_id, cliente_id) 
                                       OUTPUT INSERTED.id VALUES (GETDATE(), @tot, @uid, NULL)";
                    SqlCommand cmdV = new SqlCommand(sqlVenta, conn, trans);
                    cmdV.Parameters.AddWithValue("@tot", total);
                    cmdV.Parameters.AddWithValue("@uid", usuarioId); // ID de litos o admin
                    int ventaId = (int)cmdV.ExecuteScalar();

                    foreach (var item in items)
                    {
                        // 2. Descontar Stock
                        SqlCommand cmdUpd = new SqlCommand("UPDATE Producto SET stock = stock - @c WHERE id=@p", conn, trans);
                        cmdUpd.Parameters.AddWithValue("@c", item.Cantidad);
                        cmdUpd.Parameters.AddWithValue("@p", item.ProductoId);
                        cmdUpd.ExecuteNonQuery();

                        // 3. Registrar Movimiento (SALIDA)
                        string sqlMov = @"INSERT INTO MovimientoStock (tipoMovimiento, cantidadAnterior, cantidadNueva, usuario_id, producto_id) 
                                         VALUES ('SALIDA', 0, 0, @uid, @pid)";
                        SqlCommand cmdM = new SqlCommand(sqlMov, conn, trans);
                        cmdM.Parameters.AddWithValue("@uid", usuarioId);
                        cmdM.Parameters.AddWithValue("@pid", item.ProductoId);
                        cmdM.ExecuteNonQuery();

                        // 4. Detalle de Venta
                        SqlCommand cmdD = new SqlCommand("INSERT INTO DetalleVenta (cantidad, precioUnitario, venta_id, producto_id) VALUES (@c,@p,@v,@pid)", conn, trans);
                        cmdD.Parameters.AddWithValue("@c", item.Cantidad);
                        cmdD.Parameters.AddWithValue("@p", item.PrecioVenta);
                        cmdD.Parameters.AddWithValue("@v", ventaId);
                        cmdD.Parameters.AddWithValue("@pid", item.ProductoId);
                        cmdD.ExecuteNonQuery();
                    }
                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    System.Windows.Forms.MessageBox.Show("Error SQL: " + ex.Message); // Depuración
                    return false;
                }
            }
        }
    }
}