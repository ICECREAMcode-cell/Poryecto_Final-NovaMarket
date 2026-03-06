using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using Microsoft.Data.SqlClient;
using NovahApp.Data;

namespace NovahApp.Views
{
    public class frmGestionInventario : Form
    {
        private DataGridView dgvInv = new DataGridView();
        private NumericUpDown numCantidad = new NumericUpDown();
        private Button btnAgregar = new Button();
        private InventarioRepository _repo = new InventarioRepository();

        public frmGestionInventario()
        {
            this.Text = "Control de Inventario - NovaMarket";
            this.Size = new Size(600, 450);
            this.StartPosition = FormStartPosition.CenterScreen;

            dgvInv.Location = new Point(20, 20);
            dgvInv.Size = new Size(540, 250);
            dgvInv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInv.ReadOnly = true;

            Label lbl = new Label { Text = "Cantidad a ingresar:", Location = new Point(20, 290), Size = new Size(150, 20) };
            numCantidad.Location = new Point(20, 315);
            numCantidad.Minimum = 1;

            btnAgregar.Text = "ACTUALIZAR STOCK";
            btnAgregar.Location = new Point(160, 310);
            btnAgregar.Size = new Size(150, 35);
            btnAgregar.BackColor = Color.Navy;
            btnAgregar.ForeColor = Color.White;
            btnAgregar.Click += (s, e) => {
                if (dgvInv.CurrentRow != null) {
                    int id = (int)dgvInv.CurrentRow.Cells["id"].Value;
                    if (_repo.RegistrarEntradaStock(id, (int)numCantidad.Value)) {
                        MessageBox.Show("Stock actualizado y registrado en historial.");
                        Cargar();
                    }
                }
            };

            this.Controls.AddRange(new Control[] { dgvInv, lbl, numCantidad, btnAgregar });
            Cargar();
        }

        private void Cargar() {
            using (var conn = DbContext.Instance.GetConnection()) {
                SqlDataAdapter da = new SqlDataAdapter("SELECT id, nombre, stock FROM Producto", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvInv.DataSource = dt;
            }
        }
    }
}