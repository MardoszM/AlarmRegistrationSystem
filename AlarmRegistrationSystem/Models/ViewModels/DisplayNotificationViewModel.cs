using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Models.ViewModels
{
    public class DisplayNotificationViewModel
    {
        public Notification Notification { get; set; }

        public string MachineLocation { get; set; }
    }
}
