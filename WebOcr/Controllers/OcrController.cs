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
        private static TesseractEngine lstmEngine = new TesseractEngine(@"./tessdata", "deu", EngineMode.LstmOnly);
        private static TesseractEngine tesEngine = new TesseractEngine(@"./tessdata", "deu", EngineMode.TesseractOnly);
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
                var img = Pix.LoadTiffFromMemory(bytes);
                sw.Restart();
                var lstmPage = lstmEngine.Process(img);
                String lstmText = lstmPage.GetText();
                Console.WriteLine($"lstm engine: {sw.ElapsedMilliseconds}ms");
                lstmPage.Dispose();
                sw.Restart();
                var tesPage = tesEngine.Process(img);
                var testText = tesPage.GetText();
                Console.WriteLine($"tesEngine: {sw.ElapsedMilliseconds}ms");
                tesPage.Dispose();
                return Content(testText, "plain/text");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return new StatusCodeResult(500);
            }
        }
    }
}
