using JobsMaker.Domain;
using JobsMaker.Domain.Entities;
using JobsMaker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMaker.Applications.DTOs
{
    public class JobDTO
    {
        public Guid Id { get; set; }
        public JobType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int TotalItems { get; set; }
        public int ProcessedItems { get; set; }
        public int FailedItems { get; set; }
        public JobStatus Status { get; set; } 
        public List<string> Data { get; set; }
        public List<JobItem> Items { get; set; } = new List<JobItem>();
    }
}
