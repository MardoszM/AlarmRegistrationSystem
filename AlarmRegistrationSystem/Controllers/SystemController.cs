using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AlarmRegistrationSystem.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

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

        public IActionResult SetLanguage(string returnUrl, string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1)}
                );

            return RedirectToAction(returnUrl.GetActionFromPath(), returnUrl.GetControllerFromPath());
        }

        public bool SetLanguage2(string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            return true;
        }

        public string GetCulture()
        {
            return CultureInfo.CurrentCulture.Name;
        }
    }
}