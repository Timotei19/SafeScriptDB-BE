using Models.Entities;

namespace Data_Access_Layer.RepositoryInterfaces
{
    public interface IRoleRepository
    {
        Task AddRole(User user, int roleId);
    }
}
