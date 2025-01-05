using Microsoft.AspNetCore.Identity;

namespace Models.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public ApplicationUser()
        {
        }

        public ApplicationUser(string email)
            : base(email)
        {
        }

    }
}
