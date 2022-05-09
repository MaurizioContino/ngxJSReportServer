using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using ngxJSReportServer.Model;
using ngxJSReportServer.Services;

namespace ngxJSReportServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Report : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Get(ReportRequest req)
        {
            var stream = await Reporting.RenderJsReport(req.Content,"",req.q);
            var memory = new MemoryStream();
            await stream.CopyToAsync(memory);
            memory.Position = 0;
            return File(memory, GetContentType("report.pdf"));
        }
        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

    }
}
