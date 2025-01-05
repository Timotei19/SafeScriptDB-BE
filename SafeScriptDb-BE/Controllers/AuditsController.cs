using Business_Logic_Layer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.Entities;

namespace SafeScriptDb_BE.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuditsController : ControllerBase
    {
        private readonly IAuditService _auditService;

        public AuditsController(IAuditService auditService)
        {
            _auditService = auditService;
        }

        [HttpGet("GetAudits")]
        [ProducesResponseType(typeof(List<Audit>), 200)]
        public async Task<IActionResult> GetAudits([FromQuery] PagedRequest pagedRequest)
        {
            var audits = await _auditService.GetPagedAudits(pagedRequest);
            return Ok(audits);
        }
    }
}