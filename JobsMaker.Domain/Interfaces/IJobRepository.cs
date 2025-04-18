using JobsMaker.Applications.DTOs.Result;
using JobsMaker.Domain.Entities;
using JobsMaker.Domain.Entities.Request;
using JobsMaker.Domain.Entities.Response;
using JobsMaker.Domain.Enums;
using JobsMaker.Domain.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMaker.Domain.Interfaces;

public interface IJobRepository //: IRepository<Job>
{
    Task<IEnumerable<Job>> GetJobsAsync();
    Task<Job> GetByIdAsync(Guid? id);
    Task<Job> CreateAsync(Job job);
    Task<Job> UpdateAsync(Job job);
    Task<Job> RemoveAsync(Job job);
    Task<Job> GetJobProssesingAsync(Guid jobId);
    Task<JobStatusResponse> GetJobStatusAsync(Guid jobId); 
    Task<JobLogResponse> GetLogsAsync(Guid jobId);
    Task AddLog(JobLog jobLog);
    Task UpdateLogAsync(JobItem item);
   
}
