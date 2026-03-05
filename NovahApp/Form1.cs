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
            ConfigurarPermisosPorRol();
        }

        private void ConfigurarPermisosPorRol()
        {
            // Validamos contra la sesión activa
            string rol = UsuarioSesion.RolNombre;

            // Ajuste según tu Matriz de Roles
            btnGestionUsuarios.Visible = (rol == "Administrador");
            btnReportes.Visible = (rol == "Administrador");
            
            // Empleado y Admin gestionan stock
            btnInventario.Enabled = (rol == "Administrador" || rol == "Empleado");
            
            // Acceso general
            btnVentas.Enabled = true;

            lblBienvenida.Text = $"Usuario: {UsuarioSesion.Nombre} | Rol: {rol}";
        }

        // Navegación a los módulos que unificamos antes
        private void btnInventario_Click(object sender, EventArgs e) => new frmGestionInventario().Show();
        private void btnReportes_Click(object sender, EventArgs e) => new frmAdminDashboard().Show();
        private void btnGestionUsuarios_Click(object sender, EventArgs e)
        {
        // Creamos la instancia del panel de administración unificado   
         NovahApp.Views.frmAdminDashboard pantallaAdmin = new NovahApp.Views.frmAdminDashboard();
        // Lo abrimos como cuadro de diálogo para que no pierdas el menú principal
        pantallaAdmin.ShowDialog(); 
        }
    }       
}
