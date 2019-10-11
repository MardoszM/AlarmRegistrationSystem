using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Models.ViewModels
{
    public class ReportAccidentViewModel
    {
        public Notification Notification { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool Validate { get; set; }
    }
}
