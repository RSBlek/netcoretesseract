using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebOcr.Configuration;

namespace WebOcr.Services
{
    public class ManagementService
    {
        private readonly TessdataService tessdataService;

        public ManagementService(TessdataService tessdataService)
        {
            this.tessdataService = tessdataService;
        }

        public List<String> GetAvailableLanguages()
        {
            List<String> languages = new List<String>();
            List<Language> allLanguages = TessdataLanguages.Get().ToList();
            foreach (Language language in allLanguages)
            {
                if (tessdataService.IsDataFileAvailable(language.File))
                    languages.Add(language.Name);
            }
            return languages;
        }
    }
}
