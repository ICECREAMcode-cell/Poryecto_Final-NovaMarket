using System;
using System.Drawing;
using System.Windows.Forms;
using NovahApp.Data;

namespace NovahApp.Views
{
    public class frmRegistro : Form
    {
        private TextBox txtNombre= new TextBox(), txtEmail= new TextBox(), txtPass= new TextBox();
        private ComboBox cmbRoles= new ComboBox();
        private Button btnGuardar= new Button();
        private AuthRepository _repo = new AuthRepository();
        private bool _modoAdmin;

        public frmRegistro(bool modoAdmin = false)
        {
            _modoAdmin = modoAdmin;
            InitializeComponentManual();
        }

        private void InitializeComponentManual()
        {
            this.Text = _modoAdmin ? "Gestión de Personal/Usuarios" : "Crear Nueva Cuenta";
            this.Size = new Size(360, 480);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblNom = new Label { Text = "Nombre:", Location = new Point(30, 20) };
            txtNombre = new TextBox { Location = new Point(30, 45), Size = new Size(280, 25) };

            Label lblEm = new Label { Text = "Email:", Location = new Point(30, 90) };
            txtEmail = new TextBox { Location = new Point(30, 115), Size = new Size(280, 25) };

            Label lblPw = new Label { Text = "Password:", Location = new Point(30, 160) };
            txtPass = new TextBox { Location = new Point(30, 185), Size = new Size(280, 25), PasswordChar = '*' };

            Label lblRol = new Label { Text = "Tipo de Cuenta:", Location = new Point(30, 230), Visible = _modoAdmin };
            cmbRoles = new ComboBox { Location = new Point(30, 255), Size = new Size(280, 25), DropDownStyle = ComboBoxStyle.DropDownList, Visible = _modoAdmin };
            
            // Opciones de la barra desplegable
            cmbRoles.Items.Add(new { T = "Administrador", V = 1 });
            cmbRoles.Items.Add(new { T = "Empleado", V = 2 });
            cmbRoles.Items.Add(new { T = "Cliente", V = 3 });
            cmbRoles.DisplayMember = "T";
            cmbRoles.ValueMember = "V";
            cmbRoles.SelectedIndex = 2; // Cliente por defecto

            btnGuardar = new Button { 
                Text = "REGISTRAR USUARIO", 
                Location = new Point(30, 320), 
                Size = new Size(280, 45), 
                BackColor = Color.FromArgb(0, 122, 204), 
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat 
            };

            btnGuardar.Click += (s, e) => {
                int rolId = _modoAdmin ? ((dynamic)cmbRoles.SelectedItem).V : 3;
                if (_repo.RegistrarUsuarioCompleto(txtNombre.Text, txtEmail.Text, txtPass.Text, rolId)) {
                    MessageBox.Show("¡Guardado correctamente!", "NovaMarket");
                    this.Close();
                } else {
                    MessageBox.Show("Error al registrar.");
                }
            };

            this.Controls.AddRange(new Control[] { lblNom, txtNombre, lblEm, txtEmail, lblPw, txtPass, lblRol, cmbRoles, btnGuardar });
        }
    }
}