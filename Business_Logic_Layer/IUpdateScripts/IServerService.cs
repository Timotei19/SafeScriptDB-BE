using Models.DTOs;
using Microsoft.AspNetCore.Http;
using Models.Models;

namespace Business_Logic_Layer.IUpdateScripts
{
    public interface IServerService
    {
        public List<string> GetDatabases(DbServer credentials);

        Task<ScriptsResultModel> ExecuteSqlScripts(List<string> databases, List<IFormFile> files);
    }
}
