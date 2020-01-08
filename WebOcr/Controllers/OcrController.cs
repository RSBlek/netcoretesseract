using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        private readonly ManagementService managementService;

        public OcrController(OcrService ocrService, ManagementService managementService)
        {
            this.ocrService = ocrService;
            this.managementService = managementService;
        }

        // GET api/values
        [HttpPost]
        public async Task<IActionResult> Tiff(String language = "English", TessdataType quality = TessdataType.Normal)
        {
            if (!Request.HasFormContentType)
                return BadRequest("Content type has to be MultipartFormData");
            IFormCollection form = await Request.ReadFormAsync();
            if (form.Files.Count != 1)
                return BadRequest("No file");

            String traineddata = managementService.GetLanguageDataFileWithoutExtension(language) ?? "eng";

            byte[] bytes;
            using (MemoryStream ms = new MemoryStream())
            {
                using (Stream stream = form.Files[0].OpenReadStream())
                {
                    await stream.CopyToAsync(ms);
                    bytes = ms.ToArray();
                }
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();
            String normText = ocrService.Ocr(bytes, traineddata, quality);
            sw.Stop();
            Console.WriteLine("OCR Duration: " + sw.ElapsedMilliseconds);

            return Content(normText, "plain/text");
        }
    }
}
