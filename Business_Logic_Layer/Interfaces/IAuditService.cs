using Models.DTOs;
using Models.Entities;

namespace Business_Logic_Layer.Interfaces
{
    public interface IAuditService
    {
        Task<List<Audit>> GetAllAudits();

        Task<PagedResult<Audit>> GetPagedAudits(PagedRequest pagedRequest);
    }
}
