using JobsMaker.Applications.Interfaces;
using JobsMaker.Applications.Mappings;
using JobsMaker.Applications.Services;
using JobsMaker.Domain.Interfaces;
using JobsMaker.Infrastructure.Context;
using JobsMaker.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMaker.CrossCutting.IoC
{
    public static  class DependencyInjectionAPI
    {
        public static IServiceCollection AddInfrastructureAPI(this IServiceCollection services,
        IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
               options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                     new MySqlServerVersion(new Version(8, 0, 29))));

            services.AddScoped<IJobService, JobService>();
            services.AddScoped<IJobRepository, JobRepository>(); 
            services.AddAutoMapper(typeof(DomainToDTOMappingProfile));

            return services;
        }
    }
}
