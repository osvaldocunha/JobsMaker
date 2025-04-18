using JobsMaker.Applications.DTOs;
using JobsMaker.Applications.DTOs.Result;
using JobsMaker.Domain.Entities;
using JobsMaker.Domain.Entities.Request;
using JobsMaker.Domain.Entities.Response;
using JobsMaker.Domain.Enums;
using JobsMaker.Domain.Interfaces;
using JobsMaker.Domain.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JobsMaker.Applications.Interfaces
{
    public interface IJobService
    {
        Task<Guid> StartJobAsync(JobRequestDTO request);    
        Task<JobStatusResponseDTO> GetJobStatusAsync(Guid jobId);
        Task<IEnumerable<JobLogResponseDTO>> GetJobLogsAsync(Guid jobId);
        Task<IEnumerable<JobDTO>> GetAllAsync();
        Task<JobDTO> GetById(Guid? id);
        Task Add(JobDTO job);
        Task Update(JobDTO job);
        Task Remove(Guid? id);
    }
}
