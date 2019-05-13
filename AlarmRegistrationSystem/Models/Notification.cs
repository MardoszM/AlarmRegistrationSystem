using AlarmRegistrationSystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Models
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public string Declarant { get; set; }
        public Machine Machine { get; set; }
        public bool State { get; set; }
        public Description Description { get; set; }
    }

    public class Machine
    {
        public int MachineID { get; set; }

        public bool State { get; set; }

        [Remote(action: "VerifyId", controller: "Admin", ErrorMessage = "Takie ID istnieje w systemie.", AdditionalFields = nameof(MachineID))]
        [Required(ErrorMessage = "Proszę wporwadzić unikalny kod maszyny")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Proszę wprowadzić kod składający się co najmniej z 3 znaków")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Dozwolone znaki to [a-z][A-Z][0-9]")]
        public string MachineUniqueId { get; set; }

        [Required(ErrorMessage = "Proszę wprowadzić lokalizację maszyny")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Proszę wprowadzić Markę")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Proszę wprowadzić Model")]
        public string Model { get; set; }
    }

    public class Description
    {
        public int DescriptionID { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public List<int> Responses { get; set; }
    }
}
