using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IDatasetAnalysisService _datasetAnalysisService;

        public DatasetController(IWebHostEnvironment env, IDatasetAnalysisService datasetAnalysisService)
        {
            _env = env;
            _datasetAnalysisService = datasetAnalysisService;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload()
        {
            var files = Request.Form.Files;
            var fileInfo = await UploadDataset(files);
            var analysis = await _datasetAnalysisService.GetDatasetFeatures(fileInfo.FullName);

            var response = new
            {
                datasetFile = new
                {
                    name = Path.GetFileNameWithoutExtension(fileInfo.Name),
                    format = fileInfo.Extension,
                    size = fileInfo.Length
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
