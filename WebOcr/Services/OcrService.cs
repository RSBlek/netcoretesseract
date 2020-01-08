using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tesseract;
using WebOcr.Configuration;

namespace WebOcr.Services
{
    public class OcrService
    {
        private readonly TessdataService tessdataService;

        public OcrService(TessdataService tessdataService)
        {
            this.tessdataService = tessdataService;
        }

        public String Ocr(byte[] tiffBytes, String languageFile, TessdataType tessdataType)
        {
            String tessdataPath = tessdataService.GetPath(tessdataType);
            using (TesseractEngine engine = new TesseractEngine(tessdataPath, languageFile, EngineMode.LstmOnly))
            {
                using (Pix pix = Pix.LoadTiffFromMemory(tiffBytes))
                {
                    using (Page page = engine.Process(pix))
                    {
                        String text = page.GetText();
                        Console.WriteLine("Confidence: " + page.GetMeanConfidence());
                        return page.GetText();
                    }
                }

            }
        }


    }
}
