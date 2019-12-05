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
        private IMachineRepository machineRepository;
        private int graphSize = 10;
        private int maxDaysBack = 366;
        public ReportController(IHubContext<ChatHub> connector, IStringLocalizer<SharedResources> localizer, ILogger<BasicController> logger, INotificationRepository notificationRepository, IMachineRepository machineRepository) :base(connector, localizer, logger) 
        {
            this.notificationRepository = notificationRepository;
            this.machineRepository = machineRepository;
        }
        public IActionResult GenerateReport()
        {
            GenerateReportViewModel model = new GenerateReportViewModel() { };
            DateTime tmpDate;
            string minDateTime;
            tmpDate = notificationRepository.Notifications
                .Select(n => n.CreationDate)
                .Min();

            var value = (DateTime.Now - tmpDate).TotalDays > maxDaysBack ? true: false;
            if(value)
            {
                minDateTime = DateTime.Now.AddYears(-(maxDaysBack/366)).ToString("yyyy.MM.dd");
            }
            else
            {
                minDateTime = tmpDate.ToString("yyyy.MM.dd");
            }
            return View("GenerateReport", minDateTime);
        }
        public IActionResult MachineAllReport(string from, string to)
        {
            GraphDataViewModel model = new GraphDataViewModel()
            { ObjectName = localizer["machinename"], Value = localizer["notificationamount"], Extra = localizer["mostemergencysubassemblies"],
                DocumentationURL = "/Notification/DisplayDocumentationForMachine?machineId=", Unit = localizer["notifications"] };
            DateTime fromDate = DateTime.Parse(from);
            DateTime toDate = DateTime.Parse(to);
            model.GraphData = notificationRepository.Notifications
                .Where(n => n.CreationDate.Date >= fromDate.Date && n.CreationDate.Date <= toDate.Date)
                .GroupBy(n => n.MachineUniqueID)
                .Select(n => new GraphData { Name = n.Key, Quantity = n.Count() })
                .OrderByDescending(n => n.Quantity)
                .Take(graphSize)
                .ToList();
            foreach (var item in model.GraphData)
            {
                List<GraphData> subassemblies = (from es in notificationRepository.EmergencySubassemblies
                 join nes in notificationRepository.NotificationEs on es.Id equals nes.ESId
                 join n in notificationRepository.Notifications on nes.NotificationId equals n.NotificationID
                 where n.MachineUniqueID == item.Name
                 group es by es.Name into countes
                 select new GraphData{ Name = countes.Key, Quantity = countes.Count() })
                 .ToList();
                if (subassemblies.Count != 0)
                {
                    double max = subassemblies.Max(s => s.Quantity);
                    List<string> worstSubassemblies = subassemblies.Where(s => s.Quantity == max).Select(s => s.Name).ToList();
                    item.WorstSubassemblies = String.Join(", ", worstSubassemblies);
                }
                else
                {
                    item.WorstSubassemblies = localizer["noenoughtdata"];
                }
            }
            model.Title = localizer["machinefailures"] + " " + localizer["forperiodfrom"] + " " + DateTime.Parse(from).ToString("dd.MM.yyyy") + " " + localizer["to"] + " " + DateTime.Now.ToString("dd.MM.yyyy");
            return View("Partial/Graph",model);
        }

        public IActionResult MachineBrandReport(string from, string to)
        {
            GraphDataViewModel model = new GraphDataViewModel()
            { ObjectName = localizer["machinename"], Value = localizer["notificationamount"], Extra = localizer["mostemergencysubassemblies"],
            DocumentationURL = "/Notification/DisplayDocumentationForBrand?brand=", Unit = localizer["notifications"]};

            DateTime fromDate = DateTime.Parse(from);
            DateTime toDate = DateTime.Parse(to);
            model.GraphData = (from n in notificationRepository.Notifications
             join m in machineRepository.Machines on n.MachineUniqueID equals m.MachineUniqueId
             //where n.State == NotificationStates.Finish
             group m by m.Brand into brandNotifications
             select new GraphData { Name = brandNotifications.Key, Quantity = brandNotifications.Count() } into machines
             orderby machines.Quantity descending
             select machines)
             .Take(graphSize)
             .ToList();
            
            foreach(var item in model.GraphData)
            {
                List<GraphData> subassemblies = (from es in notificationRepository.EmergencySubassemblies
                    join nes in notificationRepository.NotificationEs on es.Id equals nes.ESId
                    join n in notificationRepository.Notifications on nes.NotificationId equals n.NotificationID
                    join m in machineRepository.Machines on n.MachineUniqueID equals m.MachineUniqueId
                    where m.Brand == item.Name //&& n.State == NotificationStates.Finish
                    group es by es.Name into countes
                    select new GraphData { Name = countes.Key, Quantity = countes.Count() })
                    .ToList();

                if (subassemblies.Count != 0)
                {
                    double max = subassemblies.Max(s => s.Quantity);
                    List<string> worstSubassemblies = subassemblies.Where(s => s.Quantity == max).Select(s => s.Name).ToList();
                    item.WorstSubassemblies = String.Join(", ", worstSubassemblies);
                }
                else
                {
                    item.WorstSubassemblies = localizer["noenoughtdata"];
                }
            }

            return View("Partial/Graph", model);
        }
    }
}