using Data_Access_Layer.ContextInterfaces;

namespace Data_Access_Layer.Contexts
{
    public class UserContext : IUserContext
    {
        public int UserId { get; set; }
    }
}
