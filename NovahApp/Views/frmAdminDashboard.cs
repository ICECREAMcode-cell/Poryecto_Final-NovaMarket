using System;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using Microsoft.Data.SqlClient; // ESTA LÍNEA FALTA Y CORRIGE EL ERROR CS0246
using NovahApp.Data;

namespace NovahApp.Views
{
    public class frmAdminDashboard : Form
    {
        private DataGridView dgv = new DataGridView();
        private Button btnCrear = new Button();
        private Button btnBan = new Button();
        private Button btnDel = new Button();
        private AuthRepository _repo = new AuthRepository();

        public frmAdminDashboard()
        {
            this.Text = "Administración de Usuarios - NovaMarket";
            this.Size = new Size(850, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            dgv.Location = new Point(20, 80);
            dgv.Size = new Size(790, 350);
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.ReadOnly = true;
            
            btnCrear.Text = "+ Agregar";
            btnCrear.Location = new Point(20, 25);
            btnCrear.Size = new Size(120, 35);
            btnCrear.Click += (s, e) => { new frmRegistro(true).ShowDialog(); Cargar(); };
            
            btnBan.Text = "🚫 Denegar/Activar";
            btnBan.Location = new Point(150, 25);
            btnBan.Size = new Size(150, 35);
            btnBan.BackColor = Color.LightSalmon;
            btnBan.Click += (s, e) => {
                if (dgv.CurrentRow != null) {
                    int id = Convert.ToInt32(dgv.CurrentRow.Cells["id"].Value);
                    bool act = Convert.ToBoolean(dgv.CurrentRow.Cells["activo"].Value);
                    // Cambiamos a CambiarEstadoUsuario para que coincida con tu Repo
                    _repo.CambiarEstadoUsuario(id, !act); 
                    Cargar();
                }
            };

            btnDel.Text = "🗑️ Eliminar Usuario";
            btnDel.Location = new Point(310, 25);
            btnDel.Size = new Size(150, 35);
            btnDel.BackColor = Color.Firebrick;
            btnDel.ForeColor = Color.White;
            btnDel.Click += (s, e) => {
                if (dgv.CurrentRow != null) {
                    int id = Convert.ToInt32(dgv.CurrentRow.Cells["id"].Value);
                    if(MessageBox.Show("¿Eliminar usuario?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                        _repo.EliminarUsuario(id);
                        Cargar();
                    }
                }
            };

            this.Controls.AddRange(new Control[] { dgv, btnCrear, btnBan, btnDel });
            Cargar();
        }

        private void Cargar()
        {
            try {
                using (var conn = DbContext.Instance.GetConnection()) {
                    // El JOIN con Rol es necesario según tu SQL
                    string sql = "SELECT u.id, u.nombre, u.email, r.nombre as Rol, u.activo FROM Usuario u JOIN Rol r ON u.rol_id = r.id";
                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgv.DataSource = dt;
                }
            } catch (Exception ex) {
                MessageBox.Show("Error al cargar: " + ex.Message);
            }
        }
    }
}