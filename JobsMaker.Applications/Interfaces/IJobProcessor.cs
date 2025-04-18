using JobsMaker.Applications.DTOs.Result;
using JobsMaker.Domain.Entities;
using JobsMaker.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMaker.Applications.Interfaces;

public interface IJobProcessor
{
    Task<JobResult> ProcessItem(object item);
}
