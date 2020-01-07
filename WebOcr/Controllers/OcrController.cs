using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Stopwatch sw = new Stopwatch();
            sw.Start();
            String base64;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                base64 = await reader.ReadToEndAsync();
            }
            Console.WriteLine($"Reading body: {sw.ElapsedMilliseconds}ms");
            sw.Restart();
            if (base64.Length == 0)
                return BadRequest();
            try
            {
                byte[] bytes = Convert.FromBase64String(base64);
                Console.WriteLine($"Convert from base64: {sw.ElapsedMilliseconds}ms");
                sw.Restart();
                var engine = new TesseractEngine(@"./tessdata", "deu", EngineMode.LstmOnly);
                Console.WriteLine($"Create tess engine: {sw.ElapsedMilliseconds}ms");
                sw.Restart();
                var img = Pix.LoadTiffFromMemory(bytes);
                Console.WriteLine($"LoadTiffFromMemory: {sw.ElapsedMilliseconds}ms");
                sw.Restart();
                var page = engine.Process(img, PageSegMode.AutoOsd);
                Console.WriteLine($"Process page: {sw.ElapsedMilliseconds}ms");
                sw.Restart();
                String text = page.GetText();
                Console.WriteLine($"GetText: {sw.ElapsedMilliseconds}ms");
                sw.Restart();
                return Content(text, "plain/text");
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }
    }
}
