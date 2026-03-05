using System;
using System.Drawing;
using System.Windows.Forms;
using NovahApp.Controllers;
using NovahApp.Models;

namespace NovahApp.Views
{
    public class frmGestionInventario : Form
    {
        private DataGridView dgvInventario;
        private NumericUpDown numCantidad;
        private Button btnCargar;
        private EmpleadoController _control = new EmpleadoController();

        public frmGestionInventario()
        {
            InitializeComponentManual();
        }

        private void InitializeComponentManual()
        {
            this.Text = "Control de Inventario - NovaMarket";
            this.Size = new Size(650, 450);

            dgvInventario = new DataGridView { Location = new Point(20, 70), Size = new Size(590, 320) };
            numCantidad = new NumericUpDown { Location = new Point(150, 25), Size = new Size(80, 25), Minimum = 1 };
            btnCargar = new Button { Text = "Registrar Entrada", Location = new Point(250, 22), Size = new Size(150, 30) };

            dgvInventario.DataSource = _control.ListarProductos();
            btnCargar.Click += (s, e) => {
                if (dgvInventario.CurrentRow != null) {
                    int id = Convert.ToInt32(dgvInventario.CurrentRow.Cells["id"].Value);
                    if (_control.AbastecerProducto(id, (int)numCantidad.Value, UsuarioSesion.Id)) {
                        MessageBox.Show("Stock actualizado");
                        dgvInventario.DataSource = _control.ListarProductos();
                    }
                }
            };

            this.Controls.AddRange(new Control[] { dgvInventario, numCantidad, btnCargar });
        }
    }
}