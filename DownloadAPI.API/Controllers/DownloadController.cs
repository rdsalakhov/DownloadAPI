using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DownloadAPI.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DownloadController : ControllerBase
    {
        // GET
        [HttpGet("file")]
        public ActionResult GetFile()
        {
            
            string destinationPath = @"/Users/romansalakhov/Downloads/downgrunge.mp4";
            Stream stream = new FileStream(destinationPath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read);

            return new FileStreamResult(stream, "application/octet-stream")
            {
                EnableRangeProcessing = true, // позволяет отдавать фрагмент файла по range заголовку
                FileDownloadName = "grunge.mp4"
            }; 
        }
    }
}