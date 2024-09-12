namespace Business_Logic_Layer.AppConstants
{
    public interface IDatabaseSettings
    {
        string GetConnectionString();

        void SetConnectionString(string server, string username, string password, string tenant = "Admin");

        string GetApplicationConnectionString();
    }
}
