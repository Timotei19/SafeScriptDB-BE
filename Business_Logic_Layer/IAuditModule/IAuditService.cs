﻿
using Models.Entities;

namespace Business_Logic_Layer.IAuditModule
{
    public interface IAuditService
    {
        Task<List<Audit>> GetAllAudits();
    }
}
