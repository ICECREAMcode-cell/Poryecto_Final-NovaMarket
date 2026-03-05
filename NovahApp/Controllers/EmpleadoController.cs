using System.Data;
using NovahApp.Data;

namespace NovahApp.Controllers
{
    public class EmpleadoController
    {
        private EmpleadoRepository _repo = new EmpleadoRepository();

        public DataTable ListarProductos() => _repo.ObtenerInventarioCompleto();

        public bool AbastecerProducto(int id, int cant, int userId) 
        {
            return _repo.RegistrarEntradaStock(id, cant, userId);
        }
    }
}