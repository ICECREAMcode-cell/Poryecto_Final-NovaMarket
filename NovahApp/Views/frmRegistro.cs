using System;
using System.Drawing;
using System.Windows.Forms;
using NovahApp.Data;

namespace NovahApp.Views
{
    public class frmRegistro : Form
    {
        private TextBox txtNombre, txtEmail, txtPass;
        private Button btnGuardar;
        private AuthRepository _repo = new AuthRepository();
        private int _rolId;

        public frmRegistro(int rolId)
        {
            _rolId = rolId;
            InitializeComponentManual();
        }

        private void InitializeComponentManual()
        {
            this.Text = (_rolId == 3) ? "Registro de Cliente" : "Nuevo Empleado";
            this.Size = new Size(350, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblNom = new Label { Text = "Nombre Completo:", Location = new Point(30, 20), Size = new Size(200, 20) };
            txtNombre = new TextBox { Location = new Point(30, 45), Size = new Size(270, 25) };

            Label lblEm = new Label { Text = "Correo Electrónico:", Location = new Point(30, 90), Size = new Size(200, 20) };
            txtEmail = new TextBox { Location = new Point(30, 115), Size = new Size(270, 25) };

            Label lblPw = new Label { Text = "Contraseña:", Location = new Point(30, 160), Size = new Size(200, 20) };
            txtPass = new TextBox { Location = new Point(30, 185), Size = new Size(270, 25), PasswordChar = '*' };

            btnGuardar = new Button { 
                Text = "CREAR CUENTA", 
                Location = new Point(30, 250), 
                Size = new Size(270, 45), 
                BackColor = Color.FromArgb(0, 122, 204), 
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat 
            };

            btnGuardar.Click += (s, e) => {
                if (_repo.RegistrarUsuarioCompleto(txtNombre.Text, txtEmail.Text, txtPass.Text, _rolId)) {
                    MessageBox.Show("¡Usuario registrado con éxito!", "NovaMarket");
                    this.Close();
                } else {
                    MessageBox.Show("Error: El correo ya existe o la conexión falló.");
                }
            };

            this.Controls.AddRange(new Control[] { lblNom, txtNombre, lblEm, txtEmail, lblPw, txtPass, btnGuardar });
        }
    }
}