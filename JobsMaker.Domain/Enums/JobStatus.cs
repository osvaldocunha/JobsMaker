using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMaker.Domain.Enums;

public enum JobStatus
{
    Pending, 
    Processing,
    InProgress,
    Completed, 
    Failed,
    Running,
    CompletedSuccessfully,
    CompletedWithErrors
}
