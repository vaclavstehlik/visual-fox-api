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

        public Task<DatasetAnalysis> GetDatasetFeatures(string datasetPath)
        {
            var result = RunProcess(datasetPath);
            return result;
        }

        private async Task<DatasetAnalysis> RunProcess(string datasetPath)
        {
            // Process configuration
            var psi = new ProcessStartInfo {FileName = Configuration.GetValue<string>("Python:Path")};
            var script = Configuration.GetValue<string>("Recommender:DatasetAnalysis:Python");

            psi.Arguments = $"{script} --path {datasetPath}";
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            
            string errors;
            string results;

            // Run process
            using (var process = Process.Start(psi))
            {
                throw new Exception("after process");
                Guard.Against.Null(process, nameof(process));

                errors = await process.StandardError.ReadToEndAsync();
                results = await process.StandardOutput.ReadToEndAsync();
            }

            if (!string.IsNullOrEmpty(errors)) throw new RecommenderExternalException("Errors occured during the analysis of dataset.");
            Guard.Against.Null(results, nameof(results));

            return JsonSerializer.Deserialize<DatasetAnalysis>(results);
        }
    }
}
