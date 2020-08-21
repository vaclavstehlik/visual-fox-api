using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Web.Interfaces;
using Web.Models;

namespace Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatasetController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private IConfiguration Configuration { get; }
        private readonly IDatasetAnalysisService _datasetAnalysisService;

        public DatasetController(IWebHostEnvironment env, IConfiguration configuration, IDatasetAnalysisService datasetAnalysisService)
        {
            _env = env;
            Configuration = configuration;
            _datasetAnalysisService = datasetAnalysisService;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload()
        {
            var file = Request.Form.Files[0];
            var content = new MultipartFormDataContent {{new StreamContent(file.OpenReadStream()), "dataset", file.FileName}};
            using var client = new HttpClient();

            var url = Configuration.GetValue<string>("Recommender:Url");
            var result = await client.PostAsync($"{url}/dataset/analysis", content);
            var analysis = JsonSerializer.Deserialize<DatasetAnalysis>(await result.Content.ReadAsStringAsync());

            var response = new
            {
                datasetFile = new
                {
                    name = Path.GetFileNameWithoutExtension(file.FileName),
                    format = Path.GetExtension(file.FileName),
                    size = file.Length
                },
                datasetAnalysis = new
                {
                    features = analysis.Features
                }
            };

            return Ok(response);
        }

        private async Task<FileInfo> UploadDataset(IFormFileCollection files)
        {
            var uploadsRootFolder = Path.Combine(_env.ContentRootPath, "Uploads");

            if (!Directory.Exists(uploadsRootFolder))
            {
                Directory.CreateDirectory(uploadsRootFolder);
            }

            var filePaths = new List<string>();

            foreach (var file in files)
            {
                if (file == null || file.Length == 0)
                {
                    continue;
                }

                var filePath = Path.Combine(uploadsRootFolder, file.FileName);
                filePaths.Add(filePath);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream).ConfigureAwait(false);
                }
            }

            var fileInfo = new List<FileInfo>();
            filePaths.ForEach(x => fileInfo.Add(new FileInfo(x)));
            return fileInfo.FirstOrDefault();
        }
    }
}
