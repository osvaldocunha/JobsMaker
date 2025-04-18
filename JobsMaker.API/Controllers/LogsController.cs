using JobsMaker.Applications.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobsMaker.API.Controllers;


[ApiController]
[Route("api/logs")]
public class LogsController : ControllerBase
{


    private readonly IJobService _jobService;
    private readonly ILogger<JobsController> _logger;

    public LogsController(IJobService jobService, ILogger<JobsController> logger)
    {
        _jobService = jobService;
        _logger = logger;
    }
    /// <summary>
    /// TODO:
    /// </summary>
    /// <param name="jobId"></param>
    /// <returns></returns>
    [HttpGet("{jobId}/logs")]
    public async Task<IActionResult> GetJobLogs(Guid jobId)
    {
        try
        {
            _logger.LogInformation($"Getting logs for job {jobId}");

            var logs = await _jobService.GetJobLogsAsync(jobId);


            if (logs == null || !logs.Any()) return NotFound();

            return Ok(logs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting job logs");
            return StatusCode(500, "Internal server error");
        }
    }

}