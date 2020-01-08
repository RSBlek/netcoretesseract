using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebOcr.Models
{
    public class WebTesseractStats
    {
        public List<String> Languages { get; set; }
        public List<String> Qualities { get; set; }
    }
}
