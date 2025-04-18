using JobsMaker.API.Controllers;
using JobsMaker.Applications.DTOs;
using JobsMaker.Applications.Interfaces;
using JobsMaker.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace JobsMaker.Testing;
public class StartJobTests : IClassFixture<JobProcessorUnitTestController>
{
    private readonly JobsController _jobsController;
    private readonly IJobService _service;

    public StartJobTests(IJobService service)
    {
        _service = service;
    }
    //public StartJobTests(JobProcessorUnitTestController controller)
    //{
    //    _jobsController = new JobsController(controller._repository);
    //}

    [Fact]
    public async Task StartJob_ValidRequest_ReturnsAccepted()
    {
        // Arrange
        var request = new JobRequestDTO
        {
            Type = JobType.Batch,
            Data = new List<string> { "test" }
        };

        var jobId = Guid.NewGuid();
       
        // Act
        var result = await _jobsController.StartJob(request);

        // Assert
        var acceptedResult = Assert.IsType<AcceptedResult>(result);
        Assert.Equal(jobId, (acceptedResult.Value as dynamic)?.JobId);
    }
}
