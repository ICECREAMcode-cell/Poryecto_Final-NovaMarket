using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Data;
using Microsoft.Data.SqlClient;
using NovahApp.Data;

namespace NovahApp.Views
{
    public class frmNuevoProducto : Form
    {
        // Controles de entrada
        private TextBox txtNombre = new TextBox();
        private NumericUpDown numCompra = new NumericUpDown();
        private NumericUpDown numVenta = new NumericUpDown();
        private NumericUpDown numStock = new NumericUpDown();
        private Label lblRutaImagen = new Label();
        private string nombreArchivoImagen = "";
        
        // Controles de visualización de stock
        private DataGridView dgvStockTotal = new DataGridView();
        private Button btnRefrescar = new Button();
        
        private ProductoRepository _repo = new ProductoRepository();

        public frmNuevoProducto()
        {
            this.Text = "Gestión de Productos y Stock - NovaMarket";
            this.Size = new Size(850, 550); // Aumentamos el ancho para que quepa la tabla
            this.StartPosition = FormStartPosition.CenterScreen;
            ConfigurarInterfaz();
            CargarTablaStock(); // Carga inicial
        }

        private void ConfigurarInterfaz()
        {
            // --- PANEL IZQUIERDO: REGISTRO ---
            int x = 30, y = 30;

            this.Controls.Add(new Label { Text = "REGISTRAR NUEVO:", Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(x, y-10) });

            this.Controls.Add(new Label { Text = "Nombre del Producto:", Location = new Point(x, y + 20) });
            txtNombre.Location = new Point(x, y + 45); txtNombre.Width = 250;
            this.Controls.Add(txtNombre);

            this.Controls.Add(new Label { Text = "Precio Compra (Bs):", Location = new Point(x, y + 80) });
            numCompra.Location = new Point(x, y + 105); numCompra.DecimalPlaces = 2;
            this.Controls.Add(numCompra);

            this.Controls.Add(new Label { Text = "Precio Venta (Bs):", Location = new Point(x + 130, y + 80) });
            numVenta.Location = new Point(x + 130, y + 105); numVenta.DecimalPlaces = 2;
            this.Controls.Add(numVenta);

            this.Controls.Add(new Label { Text = "Stock Inicial:", Location = new Point(x, y + 140) });
            numStock.Location = new Point(x, y + 165); numStock.Maximum = 10000;
            this.Controls.Add(numStock);

            Button btnImg = new Button { Text = "📸 Seleccionar Imagen", Location = new Point(x, y + 210), Size = new Size(250, 30) };
            lblRutaImagen.Location = new Point(x, y + 245); lblRutaImagen.Size = new Size(250, 40); lblRutaImagen.Text = "Sin imagen seleccionada";
            btnImg.Click += SeleccionarImagen;
            this.Controls.Add(btnImg); this.Controls.Add(lblRutaImagen);

            Button btnGuardar = new Button { 
                Text = "💾 GUARDAR PRODUCTO", 
                Location = new Point(x, y + 300), 
                Size = new Size(250, 45), 
                BackColor = Color.ForestGreen, 
                ForeColor = Color.White 
            };
            btnGuardar.Click += GuardarProducto;
            this.Controls.Add(btnGuardar);

            // --- PANEL DERECHO: VISUALIZACIÓN DE STOCK ---
            int xTabla = 320;
            this.Controls.Add(new Label { Text = "PRODUCTOS EN STOCK ACTUAL:", Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(xTabla, y-10), Size = new Size(300, 20) });

            dgvStockTotal.Location = new Point(xTabla, y + 20);
            dgvStockTotal.Size = new Size(480, 380);
            dgvStockTotal.ReadOnly = true;
            dgvStockTotal.AllowUserToAddRows = false;
            dgvStockTotal.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStockTotal.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.Controls.Add(dgvStockTotal);

            btnRefrescar.Text = "🔄 Actualizar Lista";
            btnRefrescar.Location = new Point(xTabla, y + 410);
            btnRefrescar.Size = new Size(480, 35);
            btnRefrescar.Click += (s, e) => CargarTablaStock();
            this.Controls.Add(btnRefrescar);
        }

        private void CargarTablaStock()
        {
            try {
                using (var conn = DbContext.Instance.GetConnection()) {
                    string sql = "SELECT nombre as 'Producto', stock as 'Disponible', precioVenta as 'Precio' FROM Producto ORDER BY stock ASC";
                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvStockTotal.DataSource = dt;
                }
            } catch (Exception ex) { MessageBox.Show("Error al cargar stock: " + ex.Message); }
        }

        private void SeleccionarImagen(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Imágenes|*.jpg;*.png;*.jpeg" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string carpetaAssets = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets");
                if (!Directory.Exists(carpetaAssets)) Directory.CreateDirectory(carpetaAssets);

                nombreArchivoImagen = Path.GetFileName(ofd.FileName);
                string destino = Path.Combine(carpetaAssets, nombreArchivoImagen);
                
                File.Copy(ofd.FileName, destino, true);
                lblRutaImagen.Text = "✅ Imagen: " + nombreArchivoImagen;
            }
        }

        private void GuardarProducto(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNombre.Text)) {
                MessageBox.Show("Por favor, ingrese un nombre.");
                return;
            }

            if (_repo.RegistrarNuevoProducto(txtNombre.Text, (double)numCompra.Value, (double)numVenta.Value, (int)numStock.Value, nombreArchivoImagen))
            {
                MessageBox.Show("Producto registrado y stock actualizado.");
                txtNombre.Clear();
                numCompra.Value = 0; numVenta.Value = 0; numStock.Value = 0;
                lblRutaImagen.Text = "Sin imagen seleccionada";
                CargarTablaStock(); // Refresca la tabla automáticamente al guardar
            }
        }
    }
}