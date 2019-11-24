using AlarmRegistrationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Models
{
    public class EFNotificationRepository : INotificationRepository
    {
        ApplicationDbContext context;

        public IQueryable<Notification> Notifications => context.Notifications;

        public IQueryable<Description> Descriptions => context.Descriptions;

        public IQueryable<EmergencySubassembly> EmergencySubassemblies => context.EmergencySubassemblies;

        public IQueryable<NotificationES> NotificationEs => context.NotificationEs;

        public EFNotificationRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public Notification DeleteNotification(int NotificationId)
        {
            Notification notification = null;
            try
            {
                notification = context.Notifications.FirstOrDefault(n => n.NotificationID == NotificationId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (notification != null)
            {
                try
                {
                    var descriptions = Descriptions.Where(d => d.NotificationID == NotificationId).ToList();
                    foreach (Description response in descriptions)
                    {
                        DeleteDescription(response.DescriptionID);
                    }
                    context.Notifications.Remove(notification);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return notification;
        }

        public bool SaveNotification(Notification notification)
        {
            bool value = false;
            if (notification.NotificationID == 0)
            {
                try
                {
                    context.Notifications.Add(notification);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                value = true;
            }
            else
            {
                Notification dbNotification = null;
                try
                {
                    dbNotification = context.Notifications.FirstOrDefault(n => n.NotificationID == notification.NotificationID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                if (dbNotification != null)
                {
                    dbNotification.MainDescription = notification.MainDescription;
                    dbNotification.State = notification.State;
                    value = true;
                }
            }
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public Description DeleteDescription(int DescriptionId)
        {
            Description description = null;
            try
            {
                description = context.Descriptions.FirstOrDefault(n => n.DescriptionID == DescriptionId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (description != null)
            {
                try
                {
                    context.Descriptions.Remove(description);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return description;
        }

        public bool SaveDescription(Description description)
        {
            bool value = false;
            if (description.DescriptionID == 0)
            {
                try
                {
                    context.Descriptions.Add(description);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                value = true;
            }
            else
            {
                Description dbDescription = null;
                try
                {
                    dbDescription = context.Descriptions.FirstOrDefault(d => d.DescriptionID == description.DescriptionID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                if (dbDescription != null)
                {
                    dbDescription.Text = description.Text;
                    value = true;
                }
            }
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public EmergencySubassembly DeleteSubassembly(int subassemblyId)
        {
            EmergencySubassembly subassembly = null;
            try
            {
                subassembly = context.EmergencySubassemblies.FirstOrDefault(s => s.Id == subassemblyId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (subassembly != null)
            {
                try
                {
                    context.EmergencySubassemblies.Remove(subassembly);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return subassembly;
        }

        public bool SaveSubassembly(EmergencySubassembly subassembly)
        {
            bool value = false;
            if (subassembly.Id == 0)
            {
                try
                {
                    context.EmergencySubassemblies.Add(subassembly);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                value = true;
            }
            else
            {
                EmergencySubassembly tmpsubassembly = null;
                try
                {
                    tmpsubassembly = context.EmergencySubassemblies.FirstOrDefault(s => s.Id == subassembly.Id);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                if (tmpsubassembly != null)
                {
                    tmpsubassembly.Name = subassembly.Name;
                    value = true;
                }
            }
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public NotificationES DeleteNotificationES(int notificationESId)
        {
            NotificationES notificationES = null;
            try
            {
                notificationES = context.NotificationEs.FirstOrDefault(e => e.Id == notificationESId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (notificationES != null)
            {
                try
                {
                    context.NotificationEs.Remove(notificationES);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return notificationES;
        }

        public bool SaveNotificationES(NotificationES notificationES)
        {
            bool value = false;
            if (notificationES.Id == 0)
            {
                try
                {
                    context.NotificationEs.Add(notificationES);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                value = true;
            }
            else
            {
                NotificationES tmpnotificationES = null;
                try
                {
                    tmpnotificationES = context.NotificationEs.FirstOrDefault(e => e.Id == notificationES.Id);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                if (tmpnotificationES != null)
                {
                    tmpnotificationES.NotificationId = tmpnotificationES.NotificationId;
                    tmpnotificationES.ESId = tmpnotificationES.ESId;
                    value = true;
                }
            }
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

    }
}