using Data_Access_Layer.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Data_Access_Layer.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private readonly ApplicationDbContext _context;

        public AuditRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task CreateAudit(Audit audit)
        {
            _context.Audits.Add(audit);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

        }

        public async Task<List<Audit>> GetAllAudits()
        {
            var audits = await _context.Audits.Include(x => x.AuditItems).ToListAsync();
            return audits;
        }
    }
}
