namespace SafeScriptDb_BE.AppConstants
{
    public static class DatabaseSettings
    {
        public static string ConnectionStringProvider(string selectedSqlServer, string sqlServerUsername, string sqlServerPassword, string sqlServerTennant = "ProFusion")
        {
            return $"Data Source={selectedSqlServer};Initial Catalog={sqlServerTennant};Password={sqlServerPassword};User ID={sqlServerUsername}; MultipleActiveResultSets=True;Max Pool Size=300;PoolBlockingPeriod=NeverBlock;";
        }

        public static string DefaultFolderForScriptsProvider = @"C:\Users\timotei.iancu\scriptsForDemo";

        public static string SQLServerInstanceKey = @"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL";

        public static string ServerName = "RO-2ZJ9GG3\\SQLEXPRESS";

        public static string ServerUsername = "licenta";

        public static string ServerPassword = "Audi.A4B6";
        public static string GetApplicationConnectionString()
        {
            return $"Data Source={ServerName};Initial Catalog='Admin';Password={ServerPassword};User ID={ServerUsername}; MultipleActiveResultSets=True;Max Pool Size=300;PoolBlockingPeriod=NeverBlock;TrustServerCertificate=True;Encrypt=false;";
        }
    }
}
