using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tesseract;
using WebOcr.Services;

namespace WebOcr.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OcrController : ControllerBase
    {
        private readonly OcrService ocrService;

        public OcrController(OcrService ocrService)
        {
            this.ocrService = ocrService;
        }

        // GET api/values
        [HttpPost]
        public async Task<IActionResult> Base64Tiff()
        {
            String base64;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                base64 = await reader.ReadToEndAsync();
            }
            if (base64.Length == 0)
                return BadRequest();

            byte[] bytes = Convert.FromBase64String(base64);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            String normText = ocrService.Ocr(bytes, "deu", TessdataType.Fast);
            sw.Stop();
            Console.WriteLine("OCR Duration: " + sw.ElapsedMilliseconds);

            return Content(normText, "plain/text");
        }
    }
}
