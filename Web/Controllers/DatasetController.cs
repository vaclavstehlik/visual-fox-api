using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatasetController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public DatasetController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload()
        {
            var uploadsRootFolder = Path.Combine(_env.ContentRootPath, "Uploads");
            if (!Directory.Exists(uploadsRootFolder))
            {
                Directory.CreateDirectory(uploadsRootFolder);
            }

            var files = Request.Form.Files;
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
            var data = fileInfo.Select(x => new {filename = x.Name, filesize = x.Length}).ToList();

            return Ok(new {files = data});
        }
    }
}