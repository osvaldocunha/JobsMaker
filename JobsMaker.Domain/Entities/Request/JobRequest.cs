using JobsMaker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMaker.Domain.Entities.Request;

public class JobRequest
{
    public JobType Type { get; set; }
    public List<string> DataItems { get; set; } = new();

}
