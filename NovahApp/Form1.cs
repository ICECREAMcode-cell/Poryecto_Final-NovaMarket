using System;
using System.Windows.Forms;
using NovahApp.Models;
using NovahApp.Views;

namespace NovahApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ConfigurarPermisosPorRol(); // Ejecuta la lógica de acceso al iniciar
        }

        private void ConfigurarPermisosPorRol()
        {
            string rol = UsuarioSesion.RolNombre;

    // --- MATRIZ DE ROLES CORREGIDA ---

    // 1. Catálogo: Ahora visible para TODOS (Cliente, Empleado y Admin)
    // Usamos una lógica más simple: si el rol no está vacío, que lo vea
    btnCatalogo.Visible = (rol == "Cliente" || rol == "Empleado" || rol == "Administrador");
    btnCatalogo.Enabled = true;

    // 2. Gestión de Usuarios y Reportes: SOLO el Admin
    btnGestionUsuarios.Visible = (rol == "Administrador");
    btnReportes.Visible = (rol == "Administrador");

    // 3. Inventario: El Empleado y el Admin pueden gestionar stock
    btnInventario.Enabled = (rol == "Administrador" || rol == "Empleado");
    btnInventario.Visible = (rol == "Administrador" || rol == "Empleado");

    // Etiqueta de bienvenida
    lblBienvenida.Text = $"Usuario: {UsuarioSesion.Nombre} | Rol: {rol}";

            lblBienvenida.Text = $"Usuario: {UsuarioSesion.Nombre} | Rol: {rol}";
        }

        // --- NAVEGACIÓN DE MÓDULOS ---

        //private void btnInventario_Click(object sender, EventArgs e) => new frmGestionInventario().Show();
        private void btnInventario_Click(object sender, EventArgs e)
        {
            frmNuevoProducto ventana = new frmNuevoProducto();
            ventana.ShowDialog();
        }
        private void btnReportes_Click(object sender, EventArgs e)
        {
            // Llamada al nuevo nombre del formulario
            NovahApp.Views.frmreporteDashboard pantallaReportes = new NovahApp.Views.frmreporteDashboard();
            pantallaReportes.ShowDialog();
        }

        private void btnGestionUsuarios_Click(object sender, EventArgs e)
        {
            // Panel de administración unificado
            frmAdminDashboard pantallaAdmin = new frmAdminDashboard();
            pantallaAdmin.ShowDialog(); 
        }
  

        private void btnCatalogo_Click(object sender, EventArgs e)
        {
            // Abre el catálogo con las imágenes de la carpeta Assets
            frmVentaCliente ventanaCatalogo = new frmVentaCliente();
            ventanaCatalogo.ShowDialog();
        }

        // --- GESTIÓN DE SESIÓN ---

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show("¿Está seguro de que desea cerrar sesión?", 
                                                    "Confirmación", 
                                                    MessageBoxButtons.YesNo, 
                                                    MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                // 1. Limpiamos los datos de la sesión
                UsuarioSesion.Id = 0;
                UsuarioSesion.Nombre = string.Empty;
                UsuarioSesion.RolNombre = string.Empty;

                // 2. Regresamos al Login
                frmLogin login = new frmLogin();
                login.Show();

                // 3. Cerramos el menú actual
                this.Close();
            }
        }
    }
}