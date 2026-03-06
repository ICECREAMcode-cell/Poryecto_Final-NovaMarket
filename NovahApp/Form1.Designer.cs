namespace NovahApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // Controles declarados como PUBLIC para que el Form1.cs los reconozca
        public System.Windows.Forms.Button btnGestionUsuarios;
        public System.Windows.Forms.Button btnReportes;
        public System.Windows.Forms.Button btnInventario;
        public System.Windows.Forms.Button btnVentas;
        public System.Windows.Forms.Button btnCatalogo; // Agregado para el catálogo de imágenes
        public System.Windows.Forms.Label lblBienvenida;
        public System.Windows.Forms.Button btnCerrarSesion;

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
            this.btnCatalogo = new System.Windows.Forms.Button(); // Instanciado
            this.lblBienvenida = new System.Windows.Forms.Label();
            this.btnCerrarSesion = new System.Windows.Forms.Button(); // Instanciado
            
            this.SuspendLayout();

            // lblBienvenida
            this.lblBienvenida.AutoSize = true;
            this.lblBienvenida.Location = new System.Drawing.Point(20, 20);
            this.lblBienvenida.Name = "lblBienvenida";
            this.lblBienvenida.Size = new System.Drawing.Size(250, 15);
            this.lblBienvenida.Text = "Cargando sesión...";

            // btnGestionUsuarios
            this.btnGestionUsuarios.Location = new System.Drawing.Point(50, 60);
            this.btnGestionUsuarios.Name = "btnGestionUsuarios";
            this.btnGestionUsuarios.Size = new System.Drawing.Size(250, 40);
            this.btnGestionUsuarios.Text = "Gestión de Usuarios";
            this.btnGestionUsuarios.Click += new System.EventHandler(this.btnGestionUsuarios_Click);

            // btnReportes
            this.btnReportes.Location = new System.Drawing.Point(50, 110);
            this.btnReportes.Name = "btnReportes";
            this.btnReportes.Size = new System.Drawing.Size(250, 40);
            this.btnReportes.Text = "Reportes de Ganancias";
            this.btnReportes.Click += new System.EventHandler(this.btnReportes_Click);

            // btnInventario
            this.btnInventario.Location = new System.Drawing.Point(50, 160);
            this.btnInventario.Name = "btnInventario";
            this.btnInventario.Size = new System.Drawing.Size(250, 40);
            this.btnInventario.Text = "Inventario / Stock";
            this.btnInventario.Click += new System.EventHandler(this.btnInventario_Click);

            // btnCatalogo (Ventas para Clientes)
            this.btnCatalogo.Location = new System.Drawing.Point(50, 210);
            this.btnCatalogo.Name = "btnCatalogo";
            this.btnCatalogo.Size = new System.Drawing.Size(250, 40);
            this.btnCatalogo.Text = "🛒 Ver Catálogo de Productos";
            this.btnCatalogo.BackColor = System.Drawing.Color.AliceBlue;
            this.btnCatalogo.Click += new System.EventHandler(this.btnCatalogo_Click);

            // btnCerrarSesion
            this.btnCerrarSesion.BackColor = System.Drawing.Color.LightCoral;
            this.btnCerrarSesion.Location = new System.Drawing.Point(50, 280);
            this.btnCerrarSesion.Name = "btnCerrarSesion";
            this.btnCerrarSesion.Size = new System.Drawing.Size(250, 35);
            this.btnCerrarSesion.TabIndex = 10;
            this.btnCerrarSesion.Text = "Cerrar Sesión";
            this.btnCerrarSesion.UseVisualStyleBackColor = false;
            this.btnCerrarSesion.Click += new System.EventHandler(this.btnCerrarSesion_Click);

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 350);
            this.Controls.Add(this.lblBienvenida);
            this.Controls.Add(this.btnGestionUsuarios);
            this.Controls.Add(this.btnReportes);
            this.Controls.Add(this.btnInventario);
            this.Controls.Add(this.btnCatalogo);
            this.Controls.Add(this.btnCerrarSesion);
            
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NovaMarket - Menú Principal";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}