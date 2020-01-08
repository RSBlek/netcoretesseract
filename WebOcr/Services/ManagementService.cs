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


        /// <summary>
        ///
        /// </summary>
        /// <returns>Returns null when language is not available. If language is available returns the name of the .traineddata file</returns>
        public String GetLanguageDataFileWithoutExtension(String languageName)
        {
            Language language = TessdataLanguages.Get().FirstOrDefault(x => x.Name.Equals(languageName, StringComparison.OrdinalIgnoreCase));
            if (languageName == null)
                return null;
            if (tessdataService.IsDataFileAvailable(language.File))
                return Path.GetFileNameWithoutExtension(language.File);
            else
                return null;
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
