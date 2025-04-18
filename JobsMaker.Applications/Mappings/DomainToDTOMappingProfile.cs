using AutoMapper;
using JobsMaker.Applications.DTOs;
using JobsMaker.Domain.Entities;
using JobsMaker.Domain.Entities.Request;
using JobsMaker.Domain.Entities.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMaker.Applications.Mappings;

public class DomainToDTOMappingProfile : Profile
{
    public DomainToDTOMappingProfile()
    {
        CreateMap<JobLogResponse, JobLogResponseDTO>().ReverseMap();
        CreateMap<JobRequest, JobRequestDTO>().ReverseMap();
        CreateMap<Job, JobDTO>();
    }
}
