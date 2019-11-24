using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlarmRegistrationSystem.Controllers.SystemFunctionality;
using AlarmRegistrationSystem.Hubs;
using AlarmRegistrationSystem.Infrastructure;
using AlarmRegistrationSystem.Models;
using AlarmRegistrationSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace AlarmRegistrationSystem.Controllers
{
    public class NotificationController : BasicController
    {
        private IMachineRepository machineRepository;
        private INotificationRepository notificationRepository;
        private int subassembliesPageSize = 100;

        public NotificationController(INotificationRepository notificationRepository, IMachineRepository machineRepository, ILogger<NotificationController> logger, IStringLocalizer<SharedResources> localizer, IHubContext<ChatHub> connector) :base(connector, localizer, logger)
        {
            this.machineRepository = machineRepository;
            this.notificationRepository = notificationRepository;
        }

        [Authorize]
        public IActionResult ReportAccident(int notificationId, string returnUrl = "/Home/Index")
        {
            ReportAccidentViewModel model = new ReportAccidentViewModel();
            if (notificationId != default(int))
            {
                try
                {
                    model.Notification = notificationRepository.Notifications.FirstOrDefault(n => n.NotificationID == notificationId);
                }
                catch(Exception ex)
                {
                    ErrorAlert(ex, localizer["database"], "Unable to database Exception");
                }
            }
                model.Validate = false;

            return View(model);
        }

        [HttpPost]
        public IActionResult AddNotification(Notification notification)
        {
            if (notification.NotificationID != 0)
            {
                try
                {
                    Notification tmpnotification = notificationRepository.Notifications.FirstOrDefault(n => n.NotificationID == notification.NotificationID);
                    tmpnotification.MainDescription = notification.MainDescription;
                    notificationRepository.SaveNotification(tmpnotification);

                    return RedirectToAction("DisplayNotification", new { notificationId = notification.NotificationID });
                }
                catch (Exception ex)
                {
                    ErrorAlert(ex, localizer["database"], "Unable to SaveNotification, <- Exception");
                }
            }
            else
            {
                if (ModelState.IsValid)
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
                catch (Exception ex)
                {
                    ErrorAlert(ex, localizer["database"], "Unable to SaveNotification, <- Exception");
                }
                if (!value)
                {
                    return View("ReportAccident", notification);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult DisplayNotification(int notificationID)
        {
            DisplayNotificationViewModel model = new DisplayNotificationViewModel();
            IQueryable<int> descriptionsLength = null;
            try
            {
                model.Notification = notificationRepository.Notifications
                    .FirstOrDefault(n => n.NotificationID == notificationID);
                
                model.MachineLocation = machineRepository.Machines
                    .Where(m => m.MachineUniqueId == model.Notification.MachineUniqueID)
                    .Select(m => m.Location).SingleOrDefault();

                model.Descriptions = notificationRepository.Descriptions
                    .Where(d => d.NotificationID == notificationID).ToList();
            }
            catch(Exception ex)
            {
                ErrorAlert(ex, localizer["database"], "Unable to Display notification, because of FirstOrDefault or SingleOrDefault Exception");
                //Wroc do wyswietl zgloszenia
            }
            try
            {
                descriptionsLength = notificationRepository.Descriptions
                .OrderBy(d => d.DescriptionID)
                .Where(d => d.NotificationID == notificationID)
                .Select(d => d.Text.Length);
            }
            catch (Exception ex)
            {
                ErrorAlert(ex, localizer["database"], "Unable to AddDescriptionFinally, because of database access Exception");
                model.Descriptions = null;
            }
            if (descriptionsLength != null)
            {
                int howmany = 0;
                int sum = 0;
                foreach (var desc in descriptionsLength)
                {
                    howmany++;
                    sum += desc;
                    if (sum > 1500 || howmany >= 3)
                    {
                        if(howmany < descriptionsLength.Count())
                        {
                            model.IsMore = true;
                        }
                        break;
                    }
                }
                model.Descriptions = model.Descriptions.Take(howmany).ToList();
            }
                if (model != null && model.Notification != null && model.MachineLocation != null && model.Descriptions != null)
            {
                return View(model);
            }
            else
            {
                return NotFound();
            }
        }
        public IActionResult AddDescription(int notificationId, int descriptionId)
        {
            Description description = null;
            if(descriptionId != 0)
            {
                try
                {
                    description = notificationRepository.Descriptions.FirstOrDefault(d => d.DescriptionID == descriptionId);
                }
                catch (Exception ex)
                {
                    ErrorAlert(ex, localizer["database"], "Unable to show AddDescription View to notification, because of FirstOrDefault Exception");
                }
            }
            else
            {
                description = new Description() { NotificationID = notificationId};
            }

            if (description != null)
            {
                return View(description);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult AddDescriptionFinally(Description description)
        {
                if (description.DescriptionID != 0)
                {
                    try
                    {
                        Description tmpdescription = null;
                        tmpdescription = notificationRepository.Descriptions.FirstOrDefault(d => d.DescriptionID == description.DescriptionID);
                        tmpdescription.Text = description.Text;
                        tmpdescription.LastModification = DateTime.Now;
                        tmpdescription.Author = User.Identity.Name;
                        notificationRepository.SaveDescription(tmpdescription);
                    }
                    catch (Exception ex)
                    {
                        ErrorAlert(ex, localizer["database"], "Unable to AddDescriptionFinally, because of database Exception");
                    }
                }
                else
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
                    catch (Exception ex)
                    {
                        ErrorAlert(ex, localizer["database"], "Unable to AddDescriptionFinally, because of SaveDescription Exception");
                    }
                    if (result)
                    {
                        IQueryable<int> descriptionsLength = null;
                        try
                        {
                            descriptionsLength = notificationRepository.Descriptions
                            .OrderBy(d => d.DescriptionID)
                            .Where(d => d.NotificationID == description.NotificationID)
                            .Select(d => d.Text.Length);
                        }
                        catch (Exception ex)
                        {
                            ErrorAlert(ex, localizer["database"], "Unable to AddDescriptionFinally, because of database access Exception");
                        }
                        if (descriptionsLength != null)
                        {
                            int ile = 0;
                            int suma = 0;
                            foreach (var desc in descriptionsLength)
                            {
                                ile++;
                                suma += desc;
                                if (suma > 1500 || ile > 3)
                                {
                                    //wyswietlenie dokumentacji ostatnia strona
                                    return RedirectToAction("Index", "Home");
                                }
                            }
                            int notificationId = description.NotificationID;
                            return RedirectToAction("DisplayNotification", "Notification", new { notificationId = notificationId });
                        }
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult DeleteDescription(int descriptionId)
        {
            DisplayNotificationViewModel model = new DisplayNotificationViewModel();
            try
            {
                notificationRepository.DeleteDescription(descriptionId);
                model.Descriptions = notificationRepository.Descriptions.Take(3).ToList();
            }catch(Exception ex)
            {
                ErrorAlert(ex, localizer["database"], "Unable to DeleteDescription, because of database access Exception");
                return View("Home", "Index");
            }
            return View("Partial/Description",model);
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

        [HttpPost]
        public void changeNotificationnState(int notificationId, string state)
        {
            try
            {
                Notification notification = notificationRepository.Notifications.FirstOrDefault(n => n.NotificationID == notificationId);
                if (notification != null && state != null)
                {
                    notification.State = Enum.Parse<NotificationStates>(state);
                    notificationRepository.SaveNotification(notification);
                }
            }
            catch(Exception ex)
            {
                ErrorAlert(ex, localizer["database"], "Unable to changeDescriptionState, because of database Exception");
            }
        }

        [HttpPost]
        public IActionResult AddSubassembly(string name, int notificationId = 0)
        {
            if (!(User.IsInRole("Administrators") || User.IsInRole("Mechanics")))
            {
                return Json(false);
            }
            else
            {
                if (name.Length < 5)
                {
                    return Json(false);
                }

                EmergencySubassembly ES = null;
                try
                {
                    ES = notificationRepository.EmergencySubassemblies
                        .FirstOrDefault(e => e.Name.ToLower().Replace(" ","") == name.ToLower().Replace(" ",""));
                    bool value = false;
                    if (ES == null)
                    {
                        ES = new EmergencySubassembly() { Name = name.First().ToString().ToUpper() + String.Join("", name.ToLower().Skip(1)) };
                        value = notificationRepository.SaveSubassembly(ES);
                    }
                    if (notificationId != 0)
                    {
                        NotificationES notificationES = new NotificationES() { ESId = ES.Id, NotificationId = notificationId };
                        notificationRepository.SaveNotificationES(notificationES);
                    }
                    else
                    {
                        if(value)
                        {
                            return ManageSubassemblies(null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorAlert(ex, localizer["database"], "Unable to AddSubassembly, because of database Exception");
                }
                return Json(new string[] { "false", localizer["partexist"] });
            }
        }

        [HttpPost]
        public bool DeleteNotificationSubassembly(int notificationId, string name)
        {
            if (!(User.IsInRole("Administrators") || User.IsInRole("Mechanics")))
            {
                return false;
            }
            else
            { 
                try
                {
                    int notificationES = (from nes in notificationRepository.NotificationEs
                                          join esa in notificationRepository.EmergencySubassemblies on nes.ESId equals esa.Id
                                          where nes.NotificationId == notificationId && esa.Name == name
                                          select nes.Id).FirstOrDefault();
                    notificationRepository.DeleteNotificationES(notificationES);
                }
                catch (Exception ex)
                {
                    ErrorAlert(ex, localizer["database"], "Unable to DeleteNotificationSubassembly, because of database Exception");
                    return false;
                }
                return true;
            }
        }

        public List<dynamic> GetNotificationSubassemblies(int notificationId)
        {
            List<string> notificationSubassemblies;
            List<string> allSubassemblies;
            List<dynamic> result = new List<dynamic>();
            string placeholder = localizer["placeholder"];
            string secondaryPlaceholder = localizer["secondaryplaceholder"];
            bool hasRight = false;
            if(User.IsInRole("Administrators") || User.IsInRole("Mechanics"))
            {
                hasRight = true;
            }
            try
            {
                notificationSubassemblies = (from esa in notificationRepository.EmergencySubassemblies
                                 join nes in notificationRepository.NotificationEs on esa.Id equals nes.ESId
                                 where nes.NotificationId == notificationId && esa.Name != null
                                 select esa.Name).ToList();
                allSubassemblies = notificationRepository.EmergencySubassemblies.Select(s => s.Name).ToList();

                result.Add(hasRight);
                result.Add(notificationSubassemblies);
                result.Add(allSubassemblies);
                result.Add(new List<string>() { placeholder, secondaryPlaceholder });
                return result;
            }
            catch(Exception ex)
            {
                ErrorAlert(ex, localizer["database"], "Unable to getNotificationSubassemblies, because of database Exception");
                return null;
            }
        }
    
        [Authorize]
        public IActionResult ManageSubassemblies(string searchText, int currentPage = 1)
        {
            List<EmergencySubassembly> subassemblies = null;
            int count = 0;
            try
            {
                subassemblies = notificationRepository.EmergencySubassemblies
                    .Skip(currentPage - 1)
                    .Take(subassembliesPageSize)
                    .ToList();
                if(searchText != null)
                {
                    subassemblies = subassemblies
                        .Where(e => e.Name.IsStringContains(searchText))
                        .ToList();
                }
                count = notificationRepository.EmergencySubassemblies.Count();
            }
            catch(Exception ex)
            {
                ErrorAlert(ex, localizer["database"], "Unable to ManageSubasseblies, because of database Exception");
            }
            PagingInfo pagingInfo = new PagingInfo() { 
                CurrentPage = currentPage, 
                ItemsPerPage = subassembliesPageSize, 
                TotalItems = count 
            };
            ManageSubassembliesViewModel model = new ManageSubassembliesViewModel() { 
                Subassemblies = subassemblies, 
                PagingInfo = pagingInfo
            };
            if (Request.IsAjaxRequest())
            {
                return View("Partial/Subassemblies_Table", model);
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult DeleteSubassembly(List<string> subassemblies)
        {
            bool deleteSomethig = false;
            try
            {
                foreach(string name in subassemblies)
                {
                    int id = notificationRepository.EmergencySubassemblies.Where(e => e.Name == name).Select(e => e.Id).SingleOrDefault();
                    EmergencySubassembly item = notificationRepository.DeleteSubassembly(id);
                    if(item != null)
                    {
                        NotificationES notificationES = notificationRepository.NotificationEs.Where(nes => nes.ESId == item.Id).FirstOrDefault();
                        if(notificationES != null)
                        {
                            notificationRepository.DeleteNotificationES(notificationES.Id);
                        }
                        deleteSomethig = true;
                    }
                }
                if (deleteSomethig)
                {
                    return ManageSubassemblies(null);
                }
            }
            catch(Exception ex)
            {
                ErrorAlert(ex, localizer["database"], "Unable to DeleteSubassembly, because of database Exception");
            }
                return Json(new string[] { "false", localizer["cannotdeleteanythig"] });
        }


        [HttpPost]
        public IActionResult DeleteAllUnusedSubassemblies()
        {
            try
            {
                bool deleteSomethig = false;
                IEnumerable<EmergencySubassembly> existNotificationSubassemblies = (from es in notificationRepository.EmergencySubassemblies
                                                                             join nes in notificationRepository.NotificationEs on es.Id equals nes.ESId
                                                                             select es);
                var unused = notificationRepository.EmergencySubassemblies.Except(existNotificationSubassemblies).ToList();
                
                foreach (var item in unused)
                {
                    var tmp = notificationRepository.DeleteSubassembly(item.Id);
                    if(tmp != null)
                    {
                        deleteSomethig = true;
                    }
                }
                if (deleteSomethig)
                {
                    return ManageSubassemblies(null);
                }
            }
            catch(Exception ex)
            {
                ErrorAlert(ex, localizer["database"], "Unable to DeleteAllUnusedSubassemblies, because of database Exception");
            }
            return Json(new string[]{ "false",localizer["cannotdeleteanythig"] });
        }

        [HttpPost]
        public IActionResult ExchangeParts(string part1, string part2)
        {
            try
            {
                EmergencySubassembly origin = notificationRepository.EmergencySubassemblies.Where(e => e.Name == part1).FirstOrDefault();
                EmergencySubassembly replacement = notificationRepository.EmergencySubassemblies.Where(e => e.Name == part2).FirstOrDefault();
                if(origin == null || replacement == null)
                {
                    return null;
                }

                var origins = notificationRepository.NotificationEs.Where(nes => nes.ESId == origin.Id).ToList();

                foreach(var item in origins)
                {
                    var tmp = notificationRepository.NotificationEs
                        .Where(nes => nes.ESId == replacement.Id && nes.NotificationId == item.NotificationId)
                        .FirstOrDefault();
                    if(tmp != null)
                    {
                        notificationRepository.DeleteNotificationES(origin.Id);
                    }
                    else
                    {
                        item.ESId = replacement.Id;
                        notificationRepository.SaveNotificationES(item);
                    }
                }
                notificationRepository.DeleteSubassembly(origin.Id);
                return ManageSubassemblies(null);
            }
            catch(Exception ex)
            {
                ErrorAlert(ex, localizer["database"], "Unable to ExchangeParts, because of database Exception");
            }
            return null;
        }
    }
}