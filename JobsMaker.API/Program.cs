using AutoMapper;
using JobsMaker.Applications.Interfaces;
using JobsMaker.Applications.Mappings;
using JobsMaker.Applications.Services;
using JobsMaker.Applications.Services.Processor;
using JobsMaker.Domain.Entities.Queue;
using JobsMaker.Domain.Interfaces;
using JobsMaker.Infrastructure.Context;
using JobsMaker.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSingleton<IJobService, JobService>(); 
//builder.Services.AddSingleton<IJobRepository, JobRepository>();
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddAutoMapper(typeof(Program));

string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseMySql(mySqlConnection,
                    ServerVersion.AutoDetect(mySqlConnection)));
builder.Services.AddAutoMapper(typeof(DomainToDTOMappingProfile));
builder.Services.AddSingleton<IBackgroundJobQueue, BackgroundJobQueue>();
builder.Services.AddSingleton<IJobProcessor, JobProcessor>();
//builder.Services.AddSingleton<IJobProcessingStrategy, BatchJobProcessingStrategy>();
//builder.Services.AddSingleton<IJobProcessingStrategy, BulkJobProcessingStrategy>();

//JobProcessingStrategyFactory
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
