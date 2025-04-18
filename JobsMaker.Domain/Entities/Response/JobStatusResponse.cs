using JobsMaker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMaker.Domain.Entities.Response
{
    public class JobStatusResponse
    {
        public Guid JobId { get; set; }
        public JobStatus Status { get; set; }
        public int TotalItems { get; set; }
        public int Processed { get; set; }
        public int Failed { get; set; }
    }
}
