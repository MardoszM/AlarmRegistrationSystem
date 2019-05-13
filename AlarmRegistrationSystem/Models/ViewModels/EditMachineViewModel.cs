using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Models.ViewModels
{
    public class EditMachineViewModel
    {
        public Machine Machine { get; set; }
        public string ReturnUrl { get; set; }
        public bool NewMachine { get; set; }
    }
}
