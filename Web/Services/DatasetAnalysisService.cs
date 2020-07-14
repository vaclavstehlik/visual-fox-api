using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;
using Web.Exceptions;
using Web.Interfaces;
using Web.Models;

namespace Web.Services
{
    public class DatasetAnalysisService : IDatasetAnalysisService
    {
        private IConfiguration Configuration { get; }

        public DatasetAnalysisService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Task<DatasetAnalysis> GetDatasetFeatures(string datasetPath, string appPath)
        {
            var result = RunProcess(datasetPath, appPath);
            return result;
        }

        private async Task<DatasetAnalysis> RunProcess(string datasetPath, string appPath)
        {
            // Process configuration
            var script = Configuration.GetValue<string>("Recommender:DatasetAnalysis:Python");

            var process = new Process()
            {
                StartInfo =
                {
                    FileName = Configuration.GetValue<string>("Python:Path"),
                    Arguments = $"{appPath}",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };


            string errors;
            string results;

            // Run process
            process.Start();

            // Guard.Against.Null(process, nameof(process));
            process.WaitForExit();

            errors = process.StandardError.ReadToEnd();
            results = process.StandardOutput.ReadToEnd();

            if (!string.IsNullOrEmpty(errors)) throw new RecommenderExternalException("Errors occured during the analysis of dataset.");
            Guard.Against.Null(results, nameof(results));

            return JsonSerializer.Deserialize<DatasetAnalysis>(results);
        }
    }
}