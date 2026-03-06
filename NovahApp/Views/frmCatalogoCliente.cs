using System;
using System.Drawing;
using System.Windows.Forms;
using NovahApp.Controllers;

namespace NovahApp.Views
{
    public class frmCatalogoCliente : Form
    {
        private DataGridView dgvCatalogo;
        private PictureBox picPreview;
        private Label lblPrecio;
        private ClienteController _control = new ClienteController();

        public frmCatalogoCliente()
        {
            InitializeComponentManual();
        }

        private void InitializeComponentManual()
        {
            this.Text = "Catálogo de Productos";
            this.Size = new Size(750, 450);

            dgvCatalogo = new DataGridView { Location = new Point(20, 20), Size = new Size(400, 350), SelectionMode = DataGridViewSelectionMode.FullRowSelect };
            picPreview = new PictureBox { Location = new Point(440, 20), Size = new Size(250, 250), SizeMode = PictureBoxSizeMode.Zoom, BorderStyle = BorderStyle.FixedSingle };
            lblPrecio = new Label { Text = "Precio: $0.00", Location = new Point(440, 280), Size = new Size(250, 30), Font = new Font("Arial", 14, FontStyle.Bold) };

            dgvCatalogo.DataSource = _control.VerProductos();
            dgvCatalogo.SelectionChanged += (s, e) => {
                if (dgvCatalogo.CurrentRow != null) {
                    lblPrecio.Text = $"Precio: {dgvCatalogo.CurrentRow.Cells["precioVenta"].Value} Bs.";
                    string ruta = dgvCatalogo.CurrentRow.Cells["ImagenPath"].Value.ToString();
                    try { picPreview.Image = Image.FromFile(ruta); } catch { picPreview.Image = null; }
                }
            };

            this.Controls.AddRange(new Control[] { dgvCatalogo, picPreview, lblPrecio });
        }
    }
}