using Microsoft.Data.SqlClient;
namespace NovahApp.Data
{
    public class DbContext
    {
        private static DbContext? _instance;
        private static readonly object _lock = new object();
        private readonly string _connStr = "Server=DESKTOP-PMI0ES2\\SQLEXPRESS;Database=Nova_DB;Trusted_Connection=True;TrustServerCertificate=True";
        
        private DbContext() { }


        public static DbContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new DbContext();
                    }
                }
                return _instance;
            }
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connStr);
        }
    }
}