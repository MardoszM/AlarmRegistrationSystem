using AlarmRegistrationSystem.Hubs;
using AlarmRegistrationSystem.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Controllers.SystemFunctionality
{
    public class BasicController : Controller
    {
        private IHubContext<ChatHub> connector;
        protected readonly IStringLocalizer<SharedResources> localizer;
        protected readonly ILogger<BasicController> logger;

        protected BasicController(IHubContext<ChatHub> connector, IStringLocalizer<SharedResources> localizer, ILogger<BasicController> logger)
        {
            this.connector = connector;
            this.localizer = localizer;
            this.logger = logger;
        }

        protected async void SendMessageToCaller(string message)
        {
            string[] sometext = new string[2];
            sometext[0] = "Message";
            sometext[1] = message;
            await connector.Clients.All.SendAsync("ReceiveMessage", User.Identity.Name, sometext);
        }

        protected async void SendErrorToCaller(string message)
        {
            string[] sometext = new string[2];
            sometext[0] = "Error";
            sometext[1] = message;
            await connector.Clients.All.SendAsync("ReceiveMessage", User.Identity.Name, sometext);
        }

        protected void ErrorAlert(Exception ex, string errorText, string logErrorText)
        {
            logger.LogError(ex + " || " + logErrorText);
            //SendErrorToCaller(errorText);
            TempData["Error"] = errorText;
        }
    }
}
