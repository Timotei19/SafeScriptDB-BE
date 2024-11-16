using Microsoft.SqlServer.Management.Smo;

namespace Business_Logic_Layer.AppConstants
{
    public class DatabaseSettings : IDatabaseSettings
    {
         public static string DefaultFolderForScriptsProvider = @"C:\Users\timotei.iancu\scriptsForDemo";

         public static string SQLServerInstanceKey = @"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL";

         public static string ServerName = "RO-2ZJ9GG3\\SQLEXPRESS";

         public static string ServerUsername = "licenta";

         public static string ServerPassword = "Audi.A4B6";

        private string _connectionString;

        public string GetTenantConnectionString(string tenant)
        {
            return $"Data Source={ServerName};Initial Catalog={tenant};Password={ServerPassword};User ID={ServerUsername}; MultipleActiveResultSets=True;Max Pool Size=300;PoolBlockingPeriod=NeverBlock;TrustServerCertificate=True;Encrypt=false;";
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }

        public void SetConnectionString(string server, string username, string password, string tenant = "Admin")
        {
            _connectionString = $"Data Source={server};Initial Catalog={tenant};Password={password};User ID={username}; MultipleActiveResultSets=True;Max Pool Size=300;PoolBlockingPeriod=NeverBlock;TrustServerCertificate=True;Encrypt=false;";
        }

        public static string GetApplicationConnectionString()
        {
            return $"Data Source={ServerName};Initial Catalog='Admin';Password={ServerPassword};User ID={ServerUsername}; MultipleActiveResultSets=True;Max Pool Size=300;PoolBlockingPeriod=NeverBlock;TrustServerCertificate=True;Encrypt=false;";
        }
    }
}
