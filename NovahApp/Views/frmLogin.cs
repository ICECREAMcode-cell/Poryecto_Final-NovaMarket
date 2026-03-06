using System;
using System.Drawing;
using System.Windows.Forms;
using NovahApp.Data;

namespace NovahApp.Views
{
    public partial class frmLogin : Form
    {
        private TextBox txtEmail, txtPassword = new TextBox();
        private Button btnEntrar= new Button(), btnRegistro= new Button();
        private AuthRepository _auth = new AuthRepository();

        public frmLogin()
        {
            InitializeComponentManual();
        }

        private void InitializeComponentManual()
        {
            this.Text = "Login - NovaMarket";
            this.Size = new Size(350, 420);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblEm = new Label { Text = "Correo Electrónico:", Location = new Point(40, 30), Size = new Size(200, 20) };
            txtEmail = new TextBox { Location = new Point(40, 55), Size = new Size(250, 25) };

            Label lblPw = new Label { Text = "Contraseña:", Location = new Point(40, 100), Size = new Size(200, 20) };
            txtPassword = new TextBox { Location = new Point(40, 125), Size = new Size(250, 25), PasswordChar = '*' };

            btnEntrar = new Button { 
                Text = "Entrar", 
                Location = new Point(40, 180), 
                Size = new Size(250, 40), 
                BackColor = Color.FromArgb(0, 122, 204), 
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat 
            };
            btnEntrar.Click += (s, e) => {
                if (_auth.IntentarLogin(txtEmail.Text, txtPassword.Text)) {
                    new Form1().Show();
                    this.Hide();
                } else {
                    MessageBox.Show("Acceso denegado.");
                }
            };

            btnRegistro = new Button { 
                Text = "¿No tienes cuenta? Regístrate aquí", 
                Location = new Point(40, 240), 
                Size = new Size(250, 30),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.Blue 
            };
            // Llamamos al registro con Rol 3 (Cliente)
            btnRegistro.Click += (s, e) => new frmRegistro(false).ShowDialog();

            this.Controls.AddRange(new Control[] { lblEm, txtEmail, lblPw, txtPassword, btnEntrar, btnRegistro });
        }
    }
}