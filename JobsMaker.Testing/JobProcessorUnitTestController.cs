using AutoMapper;
using JobsMaker.Applications.Mappings;
using JobsMaker.Domain.Interfaces;
using JobsMaker.Infrastructure.Context;
using JobsMaker.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMaker.Testing;

public class JobProcessorUnitTestController
{

    public IJobRepository _repository;
    public IMapper mapper;


    public static DbContextOptions<AppDbContext> dbContextOptions { get; }
    public static string connectionString =
      "Server=localhost;DataBase=JobDB;Uid=root;Pwd=bd2020";
    static JobProcessorUnitTestController()
    {
        dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
           .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
           .Options;
    }
    public JobProcessorUnitTestController()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new DomainToDTOMappingProfile());
        });

        mapper = config.CreateMapper();

        var context = new AppDbContext(dbContextOptions);
        _repository = new JobRepository(context);
    }

    public JobProcessorUnitTestController(IJobRepository repository, IMapper mapper)
    {
        _repository = repository;
        this.mapper = mapper;
    }
}
  



