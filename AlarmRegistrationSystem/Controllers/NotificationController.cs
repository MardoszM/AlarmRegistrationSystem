using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlarmRegistrationSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlarmRegistrationSystem.Controllers
{
    public class NotificationController : Controller
    {
        //private INotificationRepository repository;

        //public NotificationController(INotificationRepository repository) => this.repository = repository;

        public IActionResult ReportAccident()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNotification(Notification notification)
        {
            //TODO DODAWANIE AWARI DO BAZY
            return RedirectToAction("Index", "Home");
        }
    }
}