using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMaker.Applications.DTOs
{
    public class JobLogResponseDTO
    {
        public Guid ItemId { get; set; }
        public bool Success { get; set; }
        public string Description { get; set; }
        public DateTime? ProcessedAt { get; set; }
    }
}
