namespace NovahApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // Declaramos los controles como PUBLIC para que el .cs los encuentre siempre
        public System.Windows.Forms.Button btnGestionUsuarios;
        public System.Windows.Forms.Button btnReportes;
        public System.Windows.Forms.Button btnInventario;
        public System.Windows.Forms.Button btnVentas;
        public System.Windows.Forms.Label lblBienvenida;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnGestionUsuarios = new System.Windows.Forms.Button();
            this.btnReportes = new System.Windows.Forms.Button();
            this.btnInventario = new System.Windows.Forms.Button();
            this.btnVentas = new System.Windows.Forms.Button();
            this.lblBienvenida = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // lblBienvenida
            this.lblBienvenida.AutoSize = true;
            this.lblBienvenida.Location = new System.Drawing.Point(20, 20);
            this.lblBienvenida.Name = "lblBienvenida";
            this.lblBienvenida.Size = new System.Drawing.Size(100, 15);
            this.lblBienvenida.Text = "Bienvenido...";

            // btnGestionUsuarios
            this.btnGestionUsuarios.Location = new System.Drawing.Point(50, 60);
            this.btnGestionUsuarios.Name = "btnGestionUsuarios";
            this.btnGestionUsuarios.Size = new System.Drawing.Size(200, 40);
            this.btnGestionUsuarios.Text = "Gestión de Usuarios";
            // Busca donde se configura tu botón btnGestionUsuarios y añade esta línea:
            this.btnGestionUsuarios.Click += new System.EventHandler(this.btnGestionUsuarios_Click);

            // btnReportes
            this.btnReportes.Location = new System.Drawing.Point(50, 110);
            this.btnReportes.Name = "btnReportes";
            this.btnReportes.Size = new System.Drawing.Size(200, 40);
            this.btnReportes.Text = "Reportes de Ganancias";

            // btnInventario
            this.btnInventario.Location = new System.Drawing.Point(50, 160);
            this.btnInventario.Name = "btnInventario";
            this.btnInventario.Size = new System.Drawing.Size(200, 40);
            this.btnInventario.Text = "Inventario / Stock";
            this.btnInventario.Click += new System.EventHandler(this.btnInventario_Click);

            // btnVentas
            this.btnVentas.Location = new System.Drawing.Point(50, 210);
            this.btnVentas.Name = "btnVentas";
            this.btnVentas.Size = new System.Drawing.Size(200, 40);
            this.btnVentas.Text = "Ventas / Catálogo";

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 350);
            this.Controls.Add(this.lblBienvenida);
            this.Controls.Add(this.btnGestionUsuarios);
            this.Controls.Add(this.btnReportes);
            this.Controls.Add(this.btnInventario);
            this.Controls.Add(this.btnVentas);
            this.Name = "Form1";
            this.Text = "NovaMarket - Menú Principal";
            this.ResumeLayout(false);
            this.PerformLayout();
            //Cerrar secion
            
        }
    }
}