using JobsMaker.API.Controllers;
using JobsMaker.Applications.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMaker.Testing
{
    public class GetJobUnitTests : IClassFixture<JobProcessorUnitTestController>
    {

        private readonly JobsController _jobsController;
        private readonly IJobService _service;

        public GetJobUnitTests(IJobService service)
        {
            _service = service;
        }

        //public GetJobUnitTests(JobProcessorUnitTestController controller)
        //{
        //    _jobsController = new JobsController(controller._repository, controller.mapper );
        //}
        [Fact]
        public async Task GetJobById_OKResult()
        {
            //Arrange
  
            Guid jobId = new Guid("acf541d3-598a-47d4-a2cb-606cdc231a8b");

            //Act
            var data = _service.GetById(jobId);
           // var data = await _jobsController.GetJob(jobId);

            ////Assert
            var okResult = Assert.IsType<OkObjectResult>(data.Result);
            Assert.Equal(200, okResult.StatusCode);            
        }

        [Fact]
        public async Task GetAllJob_OKResult()
        {
            //Arrange
           
            //Act
            var data = await _jobsController.GetAllAsync();

            ////Assert
            var okResult = Assert.IsType<OkObjectResult>(data.Result);
            Assert.Equal(200, okResult.StatusCode);
        }

    }
}
