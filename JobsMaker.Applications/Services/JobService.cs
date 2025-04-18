using AutoMapper;
using JobsMaker.Applications.DTOs;
using JobsMaker.Applications.DTOs.Result;
using JobsMaker.Applications.Interfaces;
using JobsMaker.Applications.Services.Processor;
using JobsMaker.Domain.Entities;
using JobsMaker.Domain.Entities.Request;
using JobsMaker.Domain.Entities.Response;
using JobsMaker.Domain.Enums;
using JobsMaker.Domain.Interfaces;
using JobsMaker.Domain.Log;
using JobsMaker.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JobsMaker.Applications.Services;

public class JobService : IJobService
{

    private readonly IJobProcessor _externalService;
    private readonly ILogger<JobService> _logger;
    private readonly IMapper _mapper;
  
    private readonly ConcurrentQueue<Guid> _jobs = new();
    private readonly SemaphoreSlim _signal = new(0);

    private readonly IJobRepository _jobRepository;
   

    public JobService(ILogger<JobService> logger, IMapper mapper, IJobRepository jobRepository, IJobProcessor externalService)
    {    
        _logger = logger;
        _mapper = mapper;
        _jobRepository = jobRepository;
        _externalService = externalService;
    }

    public async Task<Guid> StartJob(JobType type, IEnumerable<object> items)
    {
        var job = new Job
        {
            Id = Guid.NewGuid(),
            Type = type,
            Status = JobStatus.InProgress,
            CreatedAt = DateTime.UtcNow,
            Items = items.Select(i => new JobItem
            {
                Data = JsonConvert.SerializeObject(i),
                Status = JobStatus.Pending
            }).ToList()
        };

        await _jobRepository.CreateAsync(job);

        // Start background processing
        _ = ProcessJobAsync(job.Id);

        return job.Id;
    }


    public async Task<IEnumerable<JobDTO>> GetAllAsync()
    {
        try
        {
            var jobEntity = await _jobRepository.GetJobsAsync();
            var jobDto = _mapper.Map<IEnumerable<JobDTO>>(jobEntity);
            return jobDto;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<JobDTO> GetById(Guid? id)
    {
        var jobEntity = await _jobRepository.GetByIdAsync(id);
        return _mapper.Map<JobDTO>(jobEntity);
    }

    public async Task Add(JobDTO job)
    {
        var jobEntity = _mapper.Map<Job>(job);
        await _jobRepository.CreateAsync(jobEntity);
    }

    public async Task Update(JobDTO job)
    {
        var jobEntity = _mapper.Map<Job>(job);
        await _jobRepository.UpdateAsync(jobEntity);
    }

    public async Task Remove(Guid? id)
    {
        var job = await _jobRepository.GetByIdAsync(id);
        await _jobRepository.RemoveAsync(job);  
    }

    public async Task<Guid> StartJobAsync(JobRequestDTO request)
    {
        
        var job = new Job
        {
            Id = Guid.NewGuid(),
            Type = request.Type,
            CreatedAt = DateTime.UtcNow,
            Status = JobStatus.Pending,
            TotalItems = 1// items.Count
        };


    await _jobRepository.CreateAsync(job);
        
        // Enqueue job for background processing
        EnqueueJob(job.Id);
        _ = ProcessJobAsync(job.Id);

        _logger.LogInformation($"Job {job.Id} created and enqueued for processing");

        return job.Id;
    }

    public void EnqueueJob(Guid jobId)
    {
        _jobs.Enqueue(jobId);
        _signal.Release();
    }
    public async Task<JobStatusResponseDTO> GetJobStatusAsync(Guid jobId)
    {
       var jobEntity = await _jobRepository.GetJobStatusAsync(jobId);       
      // var jobStatus = _mapper.Map<JobStatusResponse>(jobEntity);

        if (jobEntity == null) return null;

        return new JobStatusResponseDTO
        {
            JobId = jobId,
            Status = jobEntity.Status,
            TotalItems = jobEntity.TotalItems,
            ProcessedItems = jobEntity.TotalItems,
            FailedItems = jobEntity.Failed
        };
    }

    private async Task ProcessJobAsync(Guid jobId)
    {
        try
        {
            var job = await _jobRepository.GetByIdAsync(jobId);

            job.Status = JobStatus.Running;

            _logger.LogDebug($"Updating job item {job.Id}");

            await _jobRepository.UpdateAsync(job);

            foreach (var item in job.Items)
            {
                try
                {
                    item.Status = JobStatus.Processing;
                    await _jobRepository.UpdateLogAsync(item);

                    var result = await _externalService.ProcessItem(JsonConvert.DeserializeObject(item.Data));

                    item.Status = JobStatus.CompletedSuccessfully;
                    item.ProcessedAt = DateTime.UtcNow;
                    await _jobRepository.UpdateLogAsync(item);

                    await _jobRepository.AddLog(new JobLog
                    {
                        JobId = item.Id,
                        Description = "Processed successfully",
                        Timestamp = DateTime.UtcNow
                    });

                    if (job.Type == JobType.Batch)
                    {
                        throw new Exception("Item processing failed in Batch job");
                    }
                }
                catch (Exception ex)
                {
                    item.Status = JobStatus.Failed;
                    item.LogMessage = ex.Message;
                    item.ProcessedAt = DateTime.UtcNow;
                    await _jobRepository.UpdateLogAsync(item);

                    await _jobRepository.AddLog(new JobLog
                    {
                        JobId = item.Id,
                        Description = ex.Message,
                        Timestamp = DateTime.UtcNow
                    });

                    if (job.Type == JobType.Batch)
                    {
                        job.Status = JobStatus.Failed;
                        await _jobRepository.UpdateAsync(job);
                        return;
                    }
                }
            }

            job.Status = JobStatus.Completed;
            job.CompletedAt = DateTime.UtcNow;
            await _jobRepository.UpdateAsync(job);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating job item {jobId}");
            throw new Exception("Failed to update job item", ex);
        }
    }

    public async Task<IEnumerable<JobLogResponseDTO>> GetJobLogsAsync(Guid jobId)
    {
        var job = await _jobRepository.GetLogsAsync(jobId);
        
        return job.Items.Select(i => new JobLogResponseDTO
        {
            ItemId = i.Id,
            Success = i.Status == JobStatus.Completed,
            Description = i.LogMessage,
            ProcessedAt = DateTime.Today
        });
    }
}
