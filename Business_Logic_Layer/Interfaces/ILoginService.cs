using Business_Logic_Layer.Services;
using Models.Models;

namespace Business_Logic_Layer.Interfaces
{
    public interface ILoginService
    {
        Task<LoginResult> LoginUser(LoginModel login);
    }
}
