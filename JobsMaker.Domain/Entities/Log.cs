using JobsMaker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMaker.Domain.Entities;

public class Log
{
    public Guid JobId { get; set; }
    public int ItemId { get; set; }
    public JobStatus Status { get; set; }
    public string InputData { get; set; }
    public string OutputData { get; set; }
    public string ErrorDescription { get; set; }
    public DateTime? ProcessedAt { get; set; }
}

