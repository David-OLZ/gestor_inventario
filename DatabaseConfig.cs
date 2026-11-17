using Microsoft.Data.SqlClient;

namespace Practica
{
    internal static class DatabaseConfig
    {
        private static string _connectionString =
            "Data Source=LAPTOP-HP-DAVID\\SQLEXPRESS;" +
            "Initial Catalog=prueba;" +
            "Integrated Security=True;" +
            "TrustServerCertificate=True";

        public static string ConnectionString => _connectionString;

        public static void Configure(string server, string database, string? mdfPath = null)
        {
            SqlConnectionStringBuilder builder = new()
            {
                DataSource = server,
                InitialCatalog = database,
                IntegratedSecurity = true,
                TrustServerCertificate = true
            };

            if (!string.IsNullOrWhiteSpace(mdfPath))
                builder.AttachDBFilename = mdfPath;

            _connectionString = builder.ConnectionString;
        }
    }
}
