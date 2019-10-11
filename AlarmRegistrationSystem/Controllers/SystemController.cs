using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AlarmRegistrationSystem.Controllers
{
    public class SystemController : Controller
    {
        [HttpPost]
        public ViewResult CheckError()
        {
            return View("Error");
        }

        [HttpPost]
        public ViewResult CheckMessage()
        {
            return View("Message");
        }

    }
}