using Models.Models;

namespace Business_Logic_Layer.Interfaces
{
    public interface ISignInService
    {
        Task<(bool Succeeded, string Message, IEnumerable<string> Errors)> RegisterUser(RegisterModel register);
    }
}
