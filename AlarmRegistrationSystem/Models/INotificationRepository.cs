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

        bool SaveNotification(Notification notification);
        Notification DeleteNotification(int Id);

        bool SaveDescription(Description description);
        Description DeleteDescription(int Id);

    }
}
