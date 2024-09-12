namespace SafeScriptDb_BE.AppConstants
{
    public static class AppDatabaseSettings
    {
        public static string DefaultFolderForScriptsProvider = @"C:\Users\timotei.iancu\scriptsForDemo";

        public static string SQLServerInstanceKey = @"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL";

        public static string ServerName = "OMR1-LDL-IT5729\\MSSQLSERVER01";

        public static string ServerUsername = "licenta";

        public static string ServerPassword = "Audi.A4B6";

        public static string GetApplicationConnectionString()
        {
            return $"Data Source={ServerName};Initial Catalog='Admin';Password={ServerPassword};User ID={ServerUsername}; MultipleActiveResultSets=True;Max Pool Size=300;PoolBlockingPeriod=NeverBlock;TrustServerCertificate=True;Encrypt=false;";
        }
    }
}
