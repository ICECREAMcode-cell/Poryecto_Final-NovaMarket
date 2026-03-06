using System;
using System.Windows.Forms;
using NovahApp.Views; // Necesario para encontrar tu frmLogin en la carpeta Views

namespace NovahApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // IMPORTANTE: Iniciamos con el Login para cargar la sesión
            Application.Run(new frmLogin()); 
        }
    }
}