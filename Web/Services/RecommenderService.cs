using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;
using Web.Exceptions;
using Web.Interfaces;
using Web.Models.Enums;

namespace Web.Services
{
    public class RecommenderService : IRecommenderService
    {
        private IConfiguration Configuration { get; }

        private EMethodOfMCDA Method { get; }

        public RecommenderService(IConfiguration configuration)
        {
            Configuration = configuration;
            Method = EMethodOfMCDA.TOPSIS;
        }

        public Task<string> GetRanking(JsonElement userCriteria)
        {
            var result = RunProcess(userCriteria);
            return result;
        }

        private async Task<string> RunProcess(JsonElement userCriteria)
        {
            // Process configuration
            var psi = new ProcessStartInfo {FileName = Configuration.GetValue<string>("Python:Path")};
            var script = Configuration.GetValue<string>("Recommender:ScriptPath:Python");

            var sanitizedJson = Regex.Replace(userCriteria.ToString(), @"\t|\n|\r", string.Empty);
            var argumentUserCriteria = HttpUtility.JavaScriptStringEncode(sanitizedJson);
            psi.Arguments = $"{script} \"{argumentUserCriteria}\" \"{Method}\"";

            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            string errors;
            string results;

            // Run process
            using (var process = Process.Start(psi))
            {
                Guard.Against.Null(process, nameof(process));
                
                errors = await process.StandardError.ReadToEndAsync();
                results = await process.StandardOutput.ReadToEndAsync();
            }

            if (!string.IsNullOrEmpty(errors)) throw new RecommenderExternalException("Errors occured during the calculation of rankings");
            Guard.Against.Null(results, nameof(results));

            return results;
        }
    }
}