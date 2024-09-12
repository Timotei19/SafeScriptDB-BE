using Business_Logic_Layer.IAuditModule;
using Data_Access_Layer.Repositories;
using Data_Access_Layer.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
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

        public AuditService(IAuditRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }

        public async Task<List<Audit>> GetAllAudits()
        {
            return await _auditRepository.GetAllAudits();
        }
    }
}
