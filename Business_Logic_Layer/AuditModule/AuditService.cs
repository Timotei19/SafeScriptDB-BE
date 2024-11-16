using Business_Logic_Layer.IAuditModule;
using Data_Access_Layer;
using Data_Access_Layer.Repositories;
using Data_Access_Layer.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.LogsModule
{
    public class AuditService: IAuditService
    {
        private readonly IAuditRepository _auditRepository;
        private readonly ApplicationDbContext _applicationDbContext;

        public AuditService(IAuditRepository auditRepository, ApplicationDbContext applicationDbContext)
        {
            _auditRepository = auditRepository;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<List<Audit>> GetAllAudits()
        {
            return await _auditRepository.GetAllAudits();
        }

        public async Task<PagedResult<Audit>> GetPagedAudits(PagedRequest pagedRequest)
        {
            var query = _applicationDbContext.Audits.Include(x => x.AuditItems).AsQueryable();

            if (!string.IsNullOrEmpty(pagedRequest.FilterText))
            {
                query = query.Where(a => a.DatabaseName.Contains(pagedRequest.FilterText));
            }

            if (pagedRequest.StartDate.HasValue)
            {
                query = query.Where(a => a.StartDate >= pagedRequest.StartDate.Value);
            }

            if (pagedRequest.EndDate.HasValue)
            {
                query = query.Where(a => a.EndDate <= pagedRequest.EndDate.Value);
            }

            // Sorting
            query = pagedRequest.SortDesc
                ? query.OrderByDescending(e => EF.Property<object>(e, pagedRequest.SortBy))
                : query.OrderBy(e => EF.Property<object>(e, pagedRequest.SortBy));

            // Pagination
            var totalRecords = await query.CountAsync();
            var audits = await query.Skip((pagedRequest.Page - 1) * pagedRequest.PageSize)
                                    .Take(pagedRequest.PageSize)
                                    .ToListAsync();

            return new PagedResult<Audit>
            {
                Records = audits,
                TotalRecords = totalRecords
            };
        }

    }
}
