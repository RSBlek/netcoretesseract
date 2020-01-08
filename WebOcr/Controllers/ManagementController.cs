using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebOcr.Models;
using WebOcr.Services;

namespace WebOcr.Controllers
{
    [Route("/[Action]")]
    public class ManagementController : Controller
    {
        private readonly ManagementService managementService;
        public ManagementController(ManagementService managementService)
        {
            this.managementService = managementService;
        }

        [Route("/")]
        public IActionResult Index()
        {
            WebTesseractStats stats = new WebTesseractStats()
            {
                Languages = managementService.GetAvailableLanguages(),
                Qualities = Enum.GetNames(typeof(TessdataType)).ToList()
            };
            return Ok(stats);
        }

        public IActionResult Languages()
        {
            return Ok(managementService.GetAvailableLanguages());
        }

        public IActionResult Qualities()
        {
            return Ok(Enum.GetNames(typeof(TessdataType)).ToList());
        }
    }
}