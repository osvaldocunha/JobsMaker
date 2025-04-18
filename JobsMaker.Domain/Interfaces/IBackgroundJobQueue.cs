using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMaker.Domain.Interfaces;

public interface IBackgroundJobQueue
{
    void EnqueueJob(Guid jobId);
    Task<Guid> DequeueAsync(CancellationToken cancellationToken);
}
