using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Models.ViewModels
{
    public class UserListViewModel
    {
        public AppUser User { get; set; }
        public string Role { get; set; }
        public string Id { get; set; }
    }
}
