using JobsMaker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMaker.Applications.DTOs.Result;

public class JobResult
{
    public bool Success { get; set; }
    public JobType SuccessAux { get; set; }
    public string Data { get; set; }
    public string ErrorDescription { get; set; }
    public bool Fail { get; set; }

    //public List<string> Messages { get; set; }
}
