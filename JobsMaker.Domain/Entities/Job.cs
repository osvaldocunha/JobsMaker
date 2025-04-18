using JobsMaker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMaker.Domain.Entities;

public class Job
{
    public Guid Id { get; set; }
    public JobType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int TotalItems { get; set; }
    public int ProcessedItems { get; set; }
    public int FailedItems { get; set; }
    public JobStatus Status { get; set; }
    public List<string>? Data { get; set; } = new List<string>();
    public List<JobItem> Items { get; set; }  = new List<JobItem>();

    //private JobStatus CalculateStatus()
    //{
    //    if (CompletedAt.HasValue) return JobStatus.Completed;
    //    if (ProcessedItems > 0) return JobStatus.InProgress;
    //    return JobStatus.Pending;
    //}
}
