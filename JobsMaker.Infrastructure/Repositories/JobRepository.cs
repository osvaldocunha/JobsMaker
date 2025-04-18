using JobsMaker.Applications.DTOs.Result;
using JobsMaker.Domain.Entities;
using JobsMaker.Domain.Entities.Request;
using JobsMaker.Domain.Entities.Response;
using JobsMaker.Domain.Entities;
using JobsMaker.Domain.Enums;
using JobsMaker.Domain.Interfaces;
using JobsMaker.Domain.Log;
using JobsMaker.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMaker.Infrastructure.Repositories;

public class JobRepository : IJobRepository
{
    private readonly ILogger<JobRepository> _logger;
    private readonly AppDbContext _context;

    public JobRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Job>> GetJobsAsync()
    {
        try
        {
            var jobs = await _context.Jobs.AsNoTracking().ToListAsync();

            return jobs;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Job?> GetByIdAsync(Guid? id)
    {
        return await _context.Jobs.FindAsync(id);
    }

    public async Task<Job> CreateAsync(Job job)
    {
        _context.Add(job);  
        await _context.SaveChangesAsync();
        return job;
    }
    public async Task<Job> UpdateAsync(Job job)
    {
        var trackedEntity = _context.Jobs.Local.FirstOrDefault(j => j.Id == job.Id);
        if (trackedEntity != null)
        {
            _context.Entry(trackedEntity).CurrentValues.SetValues(job);
        }
        else
        {
            _context.Jobs.Update(job);
        }

        await _context.SaveChangesAsync();
        return job;
    }

    public async Task<Job> RemoveAsync(Job job)
    {
        _context.Remove(job);
        await _context.SaveChangesAsync();
        return job;
    }

    public async Task<JobStatusResponse> GetJobStatusAsync(Guid jobId)
    {
        var job = await _context.Jobs.FindAsync(jobId);
        if (job == null) return null;

        return new JobStatusResponse
        {
            TotalItems = job.Items.Count,
            Processed = job.Items.Count(i => i.ProcessedAt != null),
            Failed = job.Items.Count(i => !i.Status.Equals("Failed")),
            Status = job.Status
        };
    }

    public async Task<JobLogResponse> GetLogsAsync(Guid jobId)
    {
        // var job = _jobs[jobId];
        var job = await _context.Jobs.FindAsync(jobId);
       

        var logItems = new List<JobItem>();

        logItems.Add(new JobItem
        {
            Id = jobId,
            LogMessage = "Message of Logs",
            Status = job.Status
        });

        return new JobLogResponse
        {
            JobId = job.Id,
            Items = logItems
        };
    }

    public async  Task<Job> GetJobProssesingAsync(Guid jobId)
    {
        var job = await _context.Jobs.FindAsync(jobId);

        return await Task.FromResult(new Job
        {
            Id = job.Id,
            Status = job.Status,
            TotalItems = job.Items.Count,
            ProcessedItems = job.Items.Count(i => i.ProcessedAt != null),
            FailedItems = job.Items.Count(i => !i.Status.Equals("Failed"))
        });
    }

    public async Task AddLog(JobLog jobLog)
    {
        _context.Add(jobLog);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateLogAsync(JobItem item)
    {
        _context.JobItems.Update(item);
        await _context.SaveChangesAsync();
    }
}
