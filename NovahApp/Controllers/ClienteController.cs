using System.Data;
using NovahApp.Data;

namespace NovahApp.Controllers
{
    public class ClienteController
    {
        private ClienteRepository _repo = new ClienteRepository();

        public DataTable VerProductos() => _repo.ObtenerCatalogoPublico();
        public DataTable VerMisPedidos(int clienteId) => _repo.ObtenerMisCompras(clienteId);
    }
}