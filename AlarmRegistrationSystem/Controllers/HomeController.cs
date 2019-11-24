using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AlarmRegistrationSystem.Controllers.SystemFunctionality;
using AlarmRegistrationSystem.Hubs;
using AlarmRegistrationSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace AlarmRegistrationSystem.Controllers
{
    public class HomeController : BasicController
    {
        public HomeController(IHubContext<ChatHub> connector, IStringLocalizer<SharedResources> localizer, ILogger<HomeController> logger): base(connector, localizer, logger)
        {}

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}