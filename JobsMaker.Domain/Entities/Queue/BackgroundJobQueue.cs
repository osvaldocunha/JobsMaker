
using JobsMaker.Domain.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMaker.Domain.Entities.Queue;

public class BackgroundJobQueue : IBackgroundJobQueue
{
    private readonly ConcurrentQueue<Guid> _jobs = new();
    private readonly SemaphoreSlim _signal = new(0);

    public void EnqueueJob(Guid jobId)
    {
        _jobs.Enqueue(jobId);
        _signal.Release();
    }

    public async Task<Guid> DequeueAsync(CancellationToken cancellationToken)
    {
        await _signal.WaitAsync(cancellationToken);
        _jobs.TryDequeue(out var jobId);
        return jobId;
    }
    //private readonly ConcurrentQueue<Guid> _jobs = new ConcurrentQueue<Guid>();
    //private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

    //public async Task<Guid> DequeueAsync(CancellationToken cancellationToken)
    //{
    //    await _signal.WaitAsync(cancellationToken);
    //    _jobs.TryDequeue(out var jobId);
    //    return jobId;
    //}

    //public void EnqueueJob(Guid jobId)
    //{
    //    _jobs.Enqueue(jobId);
    //    _signal.Release();
    //}
}
