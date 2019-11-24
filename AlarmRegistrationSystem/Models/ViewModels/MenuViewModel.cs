using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Models.ViewModels
{
    public class MenuViewModel
    {
        public List<Dictionary<string, string>> Links { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
    }
}
