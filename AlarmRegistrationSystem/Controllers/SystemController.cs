using AlarmRegistrationSystem.Controllers.SystemFunctionality;
using AlarmRegistrationSystem.Hubs;
using AlarmRegistrationSystem.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;

namespace AlarmRegistrationSystem.Controllers
{
    public class SystemController :BasicController
    {
        public SystemController(IHubContext<ChatHub> connector, IStringLocalizer<SharedResources> localizer, ILogger<BasicController> logger) : base(connector, localizer, logger)
        { }

        public bool SetLanguage(string culture)
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