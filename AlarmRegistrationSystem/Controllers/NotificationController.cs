using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlarmRegistrationSystem.Infrastructure;
using AlarmRegistrationSystem.Models;
using AlarmRegistrationSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AlarmRegistrationSystem.Controllers
{
    public class NotificationController : Controller
    {
        private IMachineRepository machineRepository;
        private INotificationRepository notificationRepository;
        private ILogger<NotificationController> logger;
        private ISytemElements systemElements;

        private void ErrorAlert(Exception ex, string errorText, string logErrorText)
        {
            TempData["Error"] = errorText;
            logger.LogError(ex + " || " + logErrorText);
        }

        public NotificationController(INotificationRepository notificationRepository, IMachineRepository machineRepository, ILogger<NotificationController> logger, ISytemElements sytemElements)
        {
            this.machineRepository = machineRepository;
            this.notificationRepository = notificationRepository;
            this.logger = logger;
            this.systemElements = sytemElements;
        }

        [Authorize]
        public IActionResult ReportAccident(string returnUrl = "/Home/Index")
        {
            ReportAccidentViewModel model = new ReportAccidentViewModel()
            {
                Controller = returnUrl.GetControllerFromPath(),
                Action = returnUrl.GetActionFromPath(),
                Validate = false
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult AddNotification(Notification notification)
        
{
            if(ModelState.IsValid)
            {
                notification.CreationDate = DateTime.Now;
                notification.Declarant = User.Identity.Name;
                notification.State = NotificationStates.Aktywne;
            }
            bool value = false;
            try
            {
                value = notificationRepository.SaveNotification(notification);
            }
            catch(Exception ex)
            {
                ErrorAlert(ex, systemElements.ErrorMessages["database"], "Unable to SaveNotification, <- Exception");
            }
            if(value)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("ReportAccident", notification);
            }
        }

        [Authorize]
        public IActionResult DisplayNotification(int notificationID)
        {
            Notification model = notificationRepository.Notifications.FirstOrDefault(n => n.NotificationID == notificationID);
            if (model != null)
            {
                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        public bool VerifyMachineId(ReportAccidentViewModel model)
       {
            Machine machine = machineRepository.Machines.FirstOrDefault(m => m.MachineUniqueId == model.Notification.MachineUniqueID);
            if(model.Notification.MachineUniqueID != null)
            {
                if(machine != null)
                {
                    return true;
                }
                else
                {
                    model.Validate = true;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        public List<string> MachineCodes()
        {
            List<string> MachineCodes = machineRepository.Machines.Select(m => m.MachineUniqueId).ToList();
            return MachineCodes;
        }
    }
}