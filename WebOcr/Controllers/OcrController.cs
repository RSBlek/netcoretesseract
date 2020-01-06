using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tesseract;

namespace WebOcr.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OcrController : ControllerBase
    {
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
            try
            {
                Console.WriteLine(base64);
                byte[] bytes = Convert.FromBase64String(base64);
                var engine = new TesseractEngine(@"./tessdata", "deu", EngineMode.TesseractAndCube);
                var img = Pix.LoadTiffFromMemory(bytes);
                var page = engine.Process(img, PageSegMode.AutoOsd);
                String text = page.GetText();
                return Content(text, "plain/text");
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }
    }
}
