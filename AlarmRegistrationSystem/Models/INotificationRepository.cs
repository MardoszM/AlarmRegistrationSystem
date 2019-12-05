using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Models
{
    public interface INotificationRepository
    {
        IQueryable<Notification> Notifications { get; }
        IQueryable<Description> Descriptions { get; }

        IQueryable<EmergencySubassembly> EmergencySubassemblies { get; }

        IQueryable<NotificationES> NotificationEs { get; }

        IQueryable<JoiningPeriod> JoiningPeriods { get; }

        bool SaveNotification(Notification notification);
        Notification DeleteNotification(int Id);

        bool SaveDescription(Description description);
        Description DeleteDescription(int Id);

        bool SaveSubassembly(EmergencySubassembly emergencySubassembly);
        EmergencySubassembly DeleteSubassembly(int subassemblyId);

        bool SaveNotificationES(NotificationES notificationES);
        NotificationES DeleteNotificationES(int notificationESId);

        bool SaveJoiningPeriod(JoiningPeriod period);
        JoiningPeriod DeleteJoiningPeriod(int joiningperiodId);

    }
}
