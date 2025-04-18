    using AutoMapper;
using JobsMaker.Applications.DTOs;
using JobsMaker.Applications.Interfaces;
using JobsMaker.Applications.Services;
using JobsMaker.Domain.Entities;
using JobsMaker.Domain.Entities.Request;
using JobsMaker.Domain.Entities.Response;
using JobsMaker.Domain.Interfaces;
using JobsMaker.Domain.Log;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace JobsMaker.API.Controllers;

[ApiController]
[Route("api/jobs")]
public class JobsController : ControllerBase
{

    private readonly IJobService _jobService;
    private readonly ILogger<JobsController> _logger;
    private IJobRepository repository;
    private IMapper mapper;

    public JobsController(IJobService jobService, ILogger<JobsController> logger)
    {
        _jobService = jobService;
        _logger = logger;
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartJob([FromBody] JobRequestDTO request)
    {
        try
        {
            _logger.LogInformation($"Starting new job of type {request.Type}");
            var jobId = await _jobService.StartJobAsync(request);

            return Accepted(new { JobId = jobId });
        }
        catch (NotSupportedException ex)
        {
            _logger.LogError(ex, "Error starting job");
            return BadRequest(new { error = ex.Message });
        }
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

    //[HttpGet("{jobId}/logs")]
    //public async Task<IActionResult> GetJobLogs(Guid jobId)
    //{
    //    try
    //    {
    //        _logger.LogInformation($"Getting logs for job {jobId}");

    //        var logs = await _jobService.GetJobLogsAsync(jobId);


    //        if (logs == null || !logs.Any()) return NotFound();

    //        return Ok(logs);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Error getting job logs");
    //        return StatusCode(500, "Internal server error");
    //    }
    //}

    [HttpGet("GetAll")]
    public async Task<ActionResult<IEnumerable<JobDTO>>> GetAllAsync()
    {   
        try
        {
            var jobEntity = await _jobService.GetAllAsync();
            return Ok(jobEntity);
        }
        catch
        {
            throw;
        }
    }


    [HttpGet("{id}", Name = "GetJobById")]
    public async Task<ActionResult<JobDTO>> GetJob(Guid id)
    {
       var job = await _jobService.GetById(id);
   
        if (job is null)
        {
            return NotFound("Job not found...");
        }
        return Ok(job);
    }
   
    [HttpPut("{id:Guid}")]
    public ActionResult Put(Guid id, JobDTO job)
    {
        if (id != job.Id)
        {
            return BadRequest();//400
        }

        var jobUpdated = _jobService.Update(job);
       
        return Ok(jobUpdated);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await  _jobService.Remove(id);
        return Ok();
    }
}
