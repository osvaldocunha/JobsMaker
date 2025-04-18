using JobsMaker.Applications.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobsMaker.API.Controllers
{
    /// <summary>
    /// TODO
    /// </summary>
    [ApiController]
    [Route("api/status")]
    public class StatusController : ControllerBase
    {

        private readonly IJobService _jobService;
        private readonly ILogger<JobsController> _logger;

        public StatusController(IJobService jobService, ILogger<JobsController> logger)
        {
            _jobService = jobService;
            _logger = logger;
        }

        [HttpGet("{jobId}/status")]
        public async Task<IActionResult> GetJobStatus(Guid jobId)
        {
            try
            {
                _logger.LogInformation("Getting status for job {JobId}", jobId);
                var status = await _jobService.GetJobStatusAsync(jobId);

                if (status == null) return NotFound();

                return Ok(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting job status");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
