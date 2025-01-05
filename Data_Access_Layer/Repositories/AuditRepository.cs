using Data_Access_Layer.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Models.AppConstants;
using Models.DTOs;
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

        public async Task<UserStatisticsReponse> GetUserStatisticsAsync(UserStatisticsRequest userStatsReq)
        {
            var auditsSuccessCount = await _context.Audits
                    .Where(a => a.UserId == userStatsReq.UserId &&
                                a.StatusId == (int)Enums.Status.Finished &&
                                a.StartDate <= userStatsReq.EndDate &&
                                a.EndDate >= userStatsReq.StartDate)
                    .CountAsync();

            var auditsFailedCount = await _context.Audits
                .Where(a => a.UserId == userStatsReq.UserId &&
                            a.StatusId == (int)Enums.Status.Failed &&
                            a.StartDate <= userStatsReq.EndDate &&
                            a.EndDate >= userStatsReq.StartDate)
                .CountAsync();

            var userStatistics = new UserStatisticsReponse()
            {
                UserId = userStatsReq.UserId,
                FailedScripts = auditsFailedCount,
                SuccessScripts = auditsSuccessCount,
                StartDate = userStatsReq.StartDate,
                EndDate = userStatsReq.EndDate,
            };

            return userStatistics;
        }
    }
}
