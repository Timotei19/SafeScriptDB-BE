using Models.Models;

namespace Business_Logic_Layer.Services
{
    public class LoginResult
    {
        public ApplicationUser User { get; set; }

        public string AccessToken { get; set; }
    }
}
