using System;
using System.Data;
using System.IO;
using Microsoft.Data.SqlClient;
using NovahApp.Models;

namespace NovahApp.Data
{
    public class ReporteRepository
    {
        public double ObtenerGananciaTotal() {
            using (var conn = DbContext.Instance.GetConnection()) {
                conn.Open();
                object res = new SqlCommand("SELECT SUM(total) FROM Venta", conn).ExecuteScalar();
                return res != DBNull.Value ? Convert.ToDouble(res) : 0;
            }
        }

        public double ObtenerTotalGastos() {
            using (var conn = DbContext.Instance.GetConnection()) {
                conn.Open();
                object res = new SqlCommand("SELECT SUM(monto) FROM Gasto", conn).ExecuteScalar();
                return res != DBNull.Value ? Convert.ToDouble(res) : 0;
            }
        }

        public void RegistrarGasto(string desc, double monto) {
            using (var conn = DbContext.Instance.GetConnection()) {
                conn.Open();
                string sql = "INSERT INTO Gasto (descripcion, monto, usuario_id, fecha) VALUES (@d, @m, @u, GETDATE())";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@d", desc);
                cmd.Parameters.AddWithValue("@m", monto);
                cmd.Parameters.AddWithValue("@u", UsuarioSesion.Id);
                cmd.ExecuteNonQuery();
            }
        }

        public bool RealizarCierreMensual(double ingresos, double gastos) {
            string carpeta = @"D:\Zvscodeprojects\Facturas de compra\REPORTES_MENSUALES";
            if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);

            string ruta = Path.Combine(carpeta, $"CIERRE_{DateTime.Now:MMMM_yyyy}.txt");
            
            // Generar Reporte Automático
            string contenido = $"--- CIERRE MENSUAL NOVA MARKET ---\nFecha: {DateTime.Now}\n" +
                               $"Ingresos: {ingresos:N2} Bs.\nGastos: {gastos:N2} Bs.\n" +
                               $"Saldo Neto: {(ingresos - gastos):N2} Bs.\n" +
                               "----------------------------------";
            File.WriteAllText(ruta, contenido);

            // Limpiar Tablas
            using (var conn = DbContext.Instance.GetConnection()) {
                conn.Open();
                SqlTransaction tra = conn.BeginTransaction();
                try {
                    new SqlCommand("DELETE FROM DetalleVenta", conn, tra).ExecuteNonQuery();
                    new SqlCommand("DELETE FROM Venta", conn, tra).ExecuteNonQuery();
                    new SqlCommand("DELETE FROM Gasto", conn, tra).ExecuteNonQuery();
                    tra.Commit();
                    return true;
                } catch { tra.Rollback(); return false; }
            }
        }
    }
}