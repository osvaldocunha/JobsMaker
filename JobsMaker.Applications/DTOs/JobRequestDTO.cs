using JobsMaker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMaker.Applications.DTOs
{
    public class JobRequestDTO
    {
        [Required]
        public JobType Type { get; set; }

        [Required]
        [MinLength(1)]
        public List<string> Data { get; set; }
    }
}
