using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebOcr.Configuration;

namespace WebOcr.Services
{
    public enum TessdataType
    {
        Normal,
        Fast,
        Best
    }

    public class TessdataService
    {

        private static HashSet<String> normalFiles;
        private static HashSet<String> fastFiles;
        private static HashSet<String> bestFiles;

        private readonly TessdataPathConfiguration pathConfiguration;

        public TessdataService(IOptions<TessdataPathConfiguration> pathConfiguration)
        {
            this.pathConfiguration = pathConfiguration.Value;

            if (normalFiles == null || fastFiles == null || bestFiles == null)
                UpdateFiles();
        }

        public String GetPath(TessdataType tessdata)
        {
            if (tessdata == TessdataType.Normal)
                return pathConfiguration.Normal;
            else if (tessdata == TessdataType.Fast)
                return pathConfiguration.Fast;
            else if (tessdata == TessdataType.Best)
                return pathConfiguration.Best;
            throw new ArgumentException("Tessdata " + tessdata.ToString() + " is not available");
        }

        public void UpdateFiles()
        {
            if (Directory.Exists(pathConfiguration.Normal))
                normalFiles = Directory.GetFiles(pathConfiguration.Normal).Select(x => Path.GetFileName(x)).ToHashSet();
            else
                normalFiles = new HashSet<string>();

            if (Directory.Exists(pathConfiguration.Fast))
                fastFiles = Directory.GetFiles(pathConfiguration.Fast).Select(x => Path.GetFileName(x)).ToHashSet();
            else
                fastFiles = new HashSet<string>();

            if (Directory.Exists(pathConfiguration.Best))
                bestFiles = Directory.GetFiles(pathConfiguration.Best).Select(x => Path.GetFileName(x)).ToHashSet();
            else
                bestFiles = new HashSet<string>();
        }

        public bool IsDataFileAvailable(String file)
        {
            return normalFiles.Contains(file) && fastFiles.Contains(file) && bestFiles.Contains(file);
        }
    }
}
