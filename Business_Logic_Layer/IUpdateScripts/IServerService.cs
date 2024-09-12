using Models.DTOs;
using Microsoft.AspNetCore.Http;

namespace Business_Logic_Layer.IUpdateScripts
{
    public interface IServerService
    {
        public List<string> GetDatabases(DbServer credentials);

        Task<List<string>> ExecuteSqlScripts(List<string> databases, List<IFormFile> files);
    }
}
