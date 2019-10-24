using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AlarmRegistrationSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AlarmRegistrationSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<SharedResources> localizer;

        public HomeController(IStringLocalizer<SharedResources> localizer)
        {
            this.localizer = localizer;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}