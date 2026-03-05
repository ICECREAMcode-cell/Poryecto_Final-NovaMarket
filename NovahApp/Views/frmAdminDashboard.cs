using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using NovahApp.Controllers;

namespace NovahApp.Views
{
    public class frmAdminDashboard : Form
    {
        // Controles unificados
        private DataGridView dgvAdmin;
        private Button btnVerGanancias;
        private Label lblResumen;
        private AdminController _control = new AdminController();

        public frmAdminDashboard()
        {
            InitializeComponentManual();
        }

        private void InitializeComponentManual()
        {
            this.Text = "Panel de Administrador - NovaMarket";
            this.Size = new Size(800, 500);

            dgvAdmin = new DataGridView { Location = new Point(20, 70), Size = new Size(740, 320), Name = "dgvAdmin" };
            btnVerGanancias = new Button { Text = "Cargar Reporte Financiero", Location = new Point(20, 20), Size = new Size(200, 35) };
            lblResumen = new Label { Text = "Utilidad Neta: $0.00", Location = new Point(20, 410), Size = new Size(400, 30), Font = new Font("Arial", 12, FontStyle.Bold) };

            btnVerGanancias.Click += (s, e) => {
                DataTable dt = _control.CargarReporte();
                dgvAdmin.DataSource = dt;
                double total = 0;
                foreach (DataRow row in dt.Rows) total += Convert.ToDouble(row["UtilidadNeta"]);
                lblResumen.Text = $"Utilidad Neta Total: {total:C2}";
            };

            this.Controls.AddRange(new Control[] { dgvAdmin, btnVerGanancias, lblResumen });
            // Dentro del frmAdminDashboard:
Button btnCrearEmpleado = new Button { Text = "Registrar Empleado", Location = new Point(250, 20), Size = new Size(150, 35) };
btnCrearEmpleado.Click += (s, e) => new frmRegistro(2).ShowDialog(); // Rol 2 = Empleado
this.Controls.Add(btnCrearEmpleado);
        }
    }
}