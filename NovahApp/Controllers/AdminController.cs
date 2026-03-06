using System.Data;
using NovahApp.Data;

namespace NovahApp.Controllers
{
    public class AdminController
    {
        private AdminRepository _repo = new AdminRepository();
        public DataTable CargarReporte() => _repo.ObtenerReporteFinanciero();
    }
}