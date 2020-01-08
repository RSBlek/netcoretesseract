using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Languages()
        {
            return Ok(managementService.GetAvailableLanguages());
        }
    }
}