using JobsMaker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMaker.Applications.DTOs
{
    public class JobStatusResponseDTO
    {
        [Required]
        public Guid JobId { get; set; }
        public JobStatus Status { get; set; }
        public int TotalItems { get; set; }
        public int ProcessedItems { get; set; }
        public int FailedItems { get; set; }
    }
}
