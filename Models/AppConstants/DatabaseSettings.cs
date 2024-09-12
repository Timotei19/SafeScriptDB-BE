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

        public static string ServerName = "dev-database.cqc6jrtecjcj.eu-west-1.rds.amazonaws.com";

        public static string ServerUsername = "ProFusionAPI";

        public static string ServerPassword = "sjaMceSowooOFEyd";
    }
}
