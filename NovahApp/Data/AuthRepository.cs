using System;
using Microsoft.Data.SqlClient;
using NovahApp.Models;

namespace NovahApp.Data
{
    public class AuthRepository
    {
        public bool IntentarLogin(string email, string password)
        {
       
            using (SqlConnection conn = DbContext.Instance.GetConnection())
            {
                string sql = @"SELECT u.id, u.nombre, r.nombre AS RolNombre 
                               FROM Usuario u 
                               JOIN Rol r ON u.rol_id = r.id 
                               WHERE u.email = @email AND u.password = @pass AND u.activo = 1"; // Solo activos

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@pass", password);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        NovahApp.Models.UsuarioSesion.Id = Convert.ToInt32(reader["id"]);
                        NovahApp.Models.UsuarioSesion.Nombre = reader["nombre"].ToString() ?? "";
                        NovahApp.Models.UsuarioSesion.RolNombre = reader["RolNombre"].ToString() ?? "";
                        return true;
                    }
                }
            }
            return false;
        }

        // FUNCIÓN PARA BANEAR/DESBANEAR
        public bool CambiarEstadoUsuario(int id, bool activo) // Debe llamarse así
{
    using (SqlConnection conn = DbContext.Instance.GetConnection())
    {
        string sql = "UPDATE Usuario SET activo = @act WHERE id = @id";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@act", activo ? 1 : 0);
        cmd.Parameters.AddWithValue("@id", id);
        conn.Open();
        return cmd.ExecuteNonQuery() > 0;
    }
}

        // FUNCIÓN PARA ELIMINAR (Borra en cascada por el SQL que hiciste)
        public bool EliminarUsuario(int id)
        {
            using (SqlConnection conn = DbContext.Instance.GetConnection())
            {
                string sql = "DELETE FROM Usuario WHERE id = @id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        

        public bool RegistrarUsuarioCompleto(string nombre, string email, string password, int rolId)
        {
            using (SqlConnection conn = DbContext.Instance.GetConnection())
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    // 1. Insertar en Usuario y obtener el ID generado
                    string sqlUser = "INSERT INTO Usuario (nombre, email, password, rol_id) OUTPUT INSERTED.id VALUES (@nom, @em, @pass, @rid)";
                    SqlCommand cmdU = new SqlCommand(sqlUser, conn, trans);
                    cmdU.Parameters.AddWithValue("@nom", nombre);
                    cmdU.Parameters.AddWithValue("@em", email);
                    cmdU.Parameters.AddWithValue("@pass", password);
                    cmdU.Parameters.AddWithValue("@rid", rolId);
                    int newId = (int)cmdU.ExecuteScalar();

                    // 2. Insertar en la tabla hija correspondiente
                    string tablaHija = (rolId == 1) ? "Administrador" : (rolId == 2 ? "Empleado" : "Cliente");
                    string sqlHija = $"INSERT INTO {tablaHija} (id) VALUES (@id)";
                    SqlCommand cmdH = new SqlCommand(sqlHija, conn, trans);
                    cmdH.Parameters.AddWithValue("@id", newId);
                    cmdH.ExecuteNonQuery();

                    trans.Commit();
                    return true;
                }
                catch
                {
                    trans.Rollback();
                    return false;
                }
            }
        }
    }
}