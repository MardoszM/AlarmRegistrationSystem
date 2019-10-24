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
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace AlarmRegistrationSystem.Controllers
{
    public class NotificationController : Controller
    {
        private IMachineRepository machineRepository;
        private INotificationRepository notificationRepository;
        private ILogger<NotificationController> logger;
        private IStringLocalizer<SharedResources> localizer;

        private void ErrorAlert(Exception ex, string errorText, string logErrorText)
        {
            TempData["Error"] = errorText;
            logger.LogError(ex + " || " + logErrorText);
        }

        public NotificationController(INotificationRepository notificationRepository, IMachineRepository machineRepository, ILogger<NotificationController> logger, IStringLocalizer<SharedResources> localizer)
        {
            this.machineRepository = machineRepository;
            this.notificationRepository = notificationRepository;
            this.logger = logger;
            this.localizer = localizer;
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
                notification.State = NotificationStates.Active;
            }
            bool value = false;
            try
            {
                value = notificationRepository.SaveNotification(notification);
            }
            catch(Exception ex)
            {
                ErrorAlert(ex, localizer["database"], "Unable to SaveNotification, <- Exception");
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
            DisplayNotificationViewModel model = new DisplayNotificationViewModel();
            try
            {
                model.Notification = notificationRepository.Notifications
                    .FirstOrDefault(n => n.NotificationID == notificationID);
                
                model.MachineLocation = machineRepository.Machines
                    .Where(m => m.MachineUniqueId == model.Notification.MachineUniqueID)
                    .Select(m => m.Location).SingleOrDefault();
            }
            catch(Exception ex)
            {
                ErrorAlert(ex, localizer["database"], "Unable to Display notification, because of FirstOrDefault or SingleOrDefault Exception");
                //Wroc do wyswietl zgloszenia
            }

            if (model != null && model.Notification != null && model.MachineLocation != null)
            {
                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult AddDescription(int notificationId)
        {
            Notification model = null ;
            try
            {
                model = notificationRepository.Notifications.FirstOrDefault(n => n.NotificationID == notificationId);
            }
            catch(Exception ex)
            {
                ErrorAlert(ex, localizer["database"], "Unable to show AddDescription View to notification, because of FirstOrDefault Exception");
                //blad
            }
            if (model != null)
            {
                return View(new Description() { NotificationID = notificationId});
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult AddDescriptionFinally(Description description)
        {
            if (ModelState.IsValid)
            {
                description.Author = Request.HttpContext.User.Identity.Name;
                description.LastModification = DateTime.Now;
                bool result = false;

                try
                {
                   result = notificationRepository.SaveDescription(description);
                }
                catch(Exception ex)
                {
                    ErrorAlert(ex, localizer["database"], "Unable to AddDescriptionFinally, because of SaveDescription Exception");
                }
                if (!result)
                {
                    //wyswietlenie dokumentacji ostatnia strona
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index","Home");
        }

        public bool VerifyMachineId(ReportAccidentViewModel model)
       {
            Machine machine = null;
            try
            { 
                machine = machineRepository.Machines.FirstOrDefault(m => m.MachineUniqueId == model.Notification.MachineUniqueID);
            }
            catch(Exception ex)
            {
                ErrorAlert(ex, localizer["database"], "Unable to VerifyMachineID, because of database Exception");
                return false;
            }
            if (model.Notification.MachineUniqueID != null)
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
            List<string> machineCodes = null;
            try
            {
                machineCodes = machineRepository.Machines.Select(m => m.MachineUniqueId).ToList();
            }
            catch(Exception ex)
            {
                ErrorAlert(ex, localizer["database"], "Unable to get MachineCodes, because of database Exception");
            }
             
            return machineCodes;
        }
    }
}