using JobsMaker.Applications.DTOs.Result;
using JobsMaker.Applications.Interfaces;
using JobsMaker.Domain.Entities;
using JobsMaker.Domain.Enums;
using JobsMaker.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JobsMaker.Applications.Services.Processor;

public class JobProcessor : IJobProcessor
{
    private readonly HttpClient _httpClient;

    public JobProcessor(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public record ProcessingResult(bool IsSuccess, string Message)
    {
        public static ProcessingResult Success(string msg) => new(true, msg);
        public static ProcessingResult Fail(string msg) => new(false, msg);
    }
    private bool ValidateData(string data)
    {
        return !string.IsNullOrEmpty(data) && data.Length < 1000;
    }


    public async Task<JobResult> ProcessItem(object item)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/process", item);
            response.EnsureSuccessStatusCode();

            return new JobResult
            {
                Success = true,
                ErrorDescription = "Processed successfully"
            };
        }
        catch (Exception ex)
        {
            return new JobResult
            {
                Success = false,
                ErrorDescription = ex.Message
            };
        }
    }
}

