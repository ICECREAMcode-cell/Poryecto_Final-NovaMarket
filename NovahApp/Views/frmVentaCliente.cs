using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using NovahApp.Data;
using NovahApp.Models;
using Microsoft.Data.SqlClient; // Recomendado para .NET Core / .NET 5, 6, 7, 8

namespace NovahApp.Views
{
    public class frmVentaCliente : Form
    {
        private DataGridView dgvProductos = new DataGridView();
        private ListBox lstCarrito = new ListBox();
        private Label lblTotal = new Label();
        private Button btnComprar = new Button();
        private Button btnAgregar = new Button();
        
        private VentaRepository _ventaRepo = new VentaRepository();
        private List<CarritoItem> carrito = new List<CarritoItem>();
        private double totalGeneral = 0;

        public frmVentaCliente()
        {
            this.Text = "Catálogo NovaMarket - Sesión: " + UsuarioSesion.Nombre;
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            ConfigurarInterfaz();
            CargarProductos();
        }

        private void ConfigurarInterfaz()
        {
            // Grid de Productos
            dgvProductos.Location = new Point(20, 50);
            dgvProductos.Size = new Size(600, 400);
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.ReadOnly = true;
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Carrito
            lstCarrito.Location = new Point(640, 50);
            lstCarrito.Size = new Size(320, 350);

            // Total
            lblTotal.Text = "TOTAL: 0.00 Bs.";
            lblTotal.Location = new Point(640, 410);
            lblTotal.Font = new Font("Arial", 12, FontStyle.Bold);
            lblTotal.AutoSize = true;

            // Botón Añadir (Usa el nuevo constructor)
            btnAgregar.Text = "Añadir al Carrito";
            btnAgregar.Location = new Point(20, 470);
            btnAgregar.Size = new Size(200, 40);
            btnAgregar.Click += (s, e) => {
                if (dgvProductos.CurrentRow != null) {
                    var r = dgvProductos.CurrentRow;
                    int id = (int)r.Cells["id"].Value;
                    string nom = r.Cells["nombre"].Value.ToString();
                    double pre = Convert.ToDouble(r.Cells["precioVenta"].Value);

                    // USO DEL CONSTRUCTOR
                    carrito.Add(new CarritoItem(id, nom, 1, pre));
                    lstCarrito.Items.Add($"{nom} - {pre:N2} Bs.");
                    totalGeneral += pre;
                    lblTotal.Text = $"TOTAL: {totalGeneral:N2} Bs.";
                }
            };

            // Botón Finalizar
            btnComprar.Text = "FINALIZAR COMPRA";
            btnComprar.Location = new Point(640, 470);
            btnComprar.Size = new Size(320, 40);
            btnComprar.BackColor = Color.Green;
            btnComprar.ForeColor = Color.White;
            btnComprar.Click += (s, e) => {
                if (carrito.Count > 0) {
                    if (_ventaRepo.ProcesarCompra(UsuarioSesion.Id, carrito, totalGeneral)) {
                        MessageBox.Show("¡Venta exitosa! Stock descontado.");
                        this.Close();
                    }
                } else {
                    MessageBox.Show("El carrito está vacío.");
                }
            };

            this.Controls.AddRange(new Control[] { dgvProductos, lstCarrito, lblTotal, btnAgregar, btnComprar });
        }

        private void CargarProductos()
        {
            using (var conn = DbContext.Instance.GetConnection()) {
                SqlDataAdapter da = new SqlDataAdapter("SELECT id, nombre, precioVenta, stock FROM Producto WHERE stock > 0", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvProductos.DataSource = dt;
            }
        }
    }
}