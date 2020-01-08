using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebOcr
{
    public class Language
    {
        public String Name { get; }
        public String File { get; }
        public Language(String name, String fileName)
        {
            this.Name = name;
            this.File = fileName + ".traineddata";
        }
    }

    public static class TessdataLanguages
    {
        private static Dictionary<String, Language> nameDictionary = Get().ToDictionary(x => x.Name);

        public static Language GetByName(String name)
        {
            if (!nameDictionary.ContainsKey(name))
                return null;
            else
                return nameDictionary[name];
        }

        public static Language[] Get()
        {
            return new Language[] {
                new Language("German", "deu"),
                new Language("English", "eng")
            };
        }
    }
}