using System;
using System.Drawing;
using System.Windows.Forms;
using NovahApp.Data;

namespace NovahApp.Views
{
    public class frmreporteDashboard : Form
    {
        private ReporteRepository _repo = new ReporteRepository();
        private Label lblSaldoNeto = new Label();
        private TextBox txtMontoGasto = new TextBox();
        private TextBox txtDescGasto = new TextBox();

        public frmreporteDashboard() {
            this.Text = "Finanzas NovaMarket";
            this.Size = new Size(500, 600);
            ConfigurarInterfaz();
            CargarDatos();
        }

        private void ConfigurarInterfaz() {
            Label lblT = new Label { Text = "SALDO NETO MENSUAL", Location = new Point(20, 20), Font = new Font("Arial", 12, FontStyle.Bold) };
            lblSaldoNeto.Location = new Point(20, 50);
            lblSaldoNeto.Size = new Size(440, 80);
            lblSaldoNeto.BackColor = Color.White;
            lblSaldoNeto.Font = new Font("Arial", 25, FontStyle.Bold);
            lblSaldoNeto.TextAlign = ContentAlignment.MiddleCenter;

            // Sección Gastos
            Label lblG = new Label { Text = "Registrar Gasto (Monto y Descripción):", Location = new Point(20, 150), Size = new Size(300, 20) };
            txtMontoGasto.Location = new Point(20, 180); txtMontoGasto.Size = new Size(80, 25);
            txtDescGasto.Location = new Point(110, 180); txtDescGasto.Size = new Size(200, 25);
            
            Button btnAddGasto = new Button { Text = "Añadir Gasto", Location = new Point(320, 178), BackColor = Color.LightCoral };
            btnAddGasto.Click += (s, e) => {
                _repo.RegistrarGasto(txtDescGasto.Text, double.Parse(txtMontoGasto.Text));
                CargarDatos();
            };

            Button btnCierre = new Button { 
                Text = "CIERRE MENSUAL (Guardar .txt y Reiniciar)", 
                Location = new Point(20, 480), Size = new Size(440, 45), 
                BackColor = Color.DarkRed, ForeColor = Color.White 
            };
            btnCierre.Click += (s, e) => {
                if (_repo.RealizarCierreMensual(_repo.ObtenerGananciaTotal(), _repo.ObtenerTotalGastos())) {
                    MessageBox.Show("Reporte guardado en D: y tablas limpias.");
                    CargarDatos();
                }
            };

            this.Controls.AddRange(new Control[] { lblT, lblSaldoNeto, lblG, txtMontoGasto, txtDescGasto, btnAddGasto, btnCierre });
        }

        private void CargarDatos() {
            double ing = _repo.ObtenerGananciaTotal();
            double gas = _repo.ObtenerTotalGastos();
            lblSaldoNeto.Text = $"{(ing - gas):N2} Bs.";
        }
    }
}