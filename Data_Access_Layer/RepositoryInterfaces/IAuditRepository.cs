using Models.Entities;

namespace Data_Access_Layer.RepositoryInterfaces
{
    public interface IAuditRepository
    {
        public Task CreateAudit(Audit audit);

        public Task<List<Audit>> GetAllAudits();
    }
}
