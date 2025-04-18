using JobsMaker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMaker.Domain.Entities;

public class JobItem
{
    public Guid Id { get; set; }
    public string Data { get; set; }
    public JobStatus Status { get; set; } 
    public string? LogMessage { get; set; }
    public DateTime? ProcessedAt { get; set; }
}
