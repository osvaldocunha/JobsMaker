using JobsMaker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMaker.Domain.Entities.Response;

public class JobLogResponse
{
    public Guid JobId { get; set; }
    public List<JobItem> Items { get; set; } = new();

    public Guid ItemId { get; set; }
    public bool Success { get; set; }
    public string Description { get; set; }
    public DateTime? ProcessedAt { get; set; }

}