using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlarmRegistrationSystem.Controllers.SystemFunctionality;
using AlarmRegistrationSystem.Hubs;
using AlarmRegistrationSystem.Models;
using AlarmRegistrationSystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace AlarmRegistrationSystem.Controllers
{
    public class ReportController : BasicController
    {
        private INotificationRepository notificationRepository;
        private int graphSize = 10;
        private int maxDaysBack = 366;
        public ReportController(IHubContext<ChatHub> connector, IStringLocalizer<SharedResources> localizer, ILogger<BasicController> logger, INotificationRepository notificationRepository) :base(connector, localizer, logger) 
        {
            this.notificationRepository = notificationRepository;
        }
        public IActionResult GenerateReport()
        {
            GenerateReportViewModel model = new GenerateReportViewModel() { };
            model.Methods.Add(new string[] { "MachineAllReport", localizer["machineallreport"] });
            model.Methods.Add(new string[] { "MachineBrand", localizer["machinebrand"] });
            model.Methods.Add(new string[] { "MachineBrandModel", localizer["machinebrandmodel"] });

            var MinDateTime = notificationRepository.Notifications
                .Select(n => n.CreationDate)
                .Min();

            var value = (DateTime.Now - MinDateTime).TotalDays > maxDaysBack ? true: false;
            if(value)
            {
                model.MinDateTime = DateTime.Now.AddYears(-(maxDaysBack/366)).ToString("yyyy.MM.dd");
            }
            else
            {
                model.MinDateTime = MinDateTime.ToString("yyyy.MM.dd");
            }
            return View(model);
        }

        public IActionResult MachineAllReport(string from, string to)
        {
            GraphDataViewModel model = new GraphDataViewModel() { ObjectName = localizer["machinename"], Value = localizer["notificationamount"]};
            DateTime fromDate = DateTime.Parse(from);
            DateTime toDate = DateTime.Parse(to);
            model.GraphData = notificationRepository.Notifications
                .Where(n => n.CreationDate.Date >= fromDate.Date && n.CreationDate.Date <= toDate.Date)
                .GroupBy(n => n.MachineUniqueID)
                .Select(n => new GraphData { Name = n.Key, Quantity = n.Count() })
                .OrderByDescending(n => n.Quantity)
                .Take(graphSize)
                .ToList();
            return View("Partial/Graph",model);
        }
    }
}