using AlarmRegistrationSystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Models
{
    public enum NotificationStates
    {
        Aktywne, W_naprawie, Wstrzymane, Zakonczone
    }

    public class Notification
    {
        public int NotificationID { get; set; }
        public string Declarant { get; set; }
        [Remote(action:"VerifyMachineId", controller: "Notification", ErrorMessage = "Maszyna o takim kodzie nie istnieje w systemie", AdditionalFields = nameof(MachineUniqueID))]
        [Required(ErrorMessage = "Proszę wprowadzić kod Maszyny z usterką")]
        public string MachineUniqueID { get; set; }
        public NotificationStates State { get; set; }
        [MinLength(20, ErrorMessage = "Opis usterki musi być dluzszy")]
        [MaxLength(1000, ErrorMessage = "Opis usterki musi być krotszy")]
        [Required(ErrorMessage = "Proszę wprowadzić opis awarii")]
        public string MainDescription { get; set; }
        public DateTime CreationDate { get; set; }

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
        public int NotificationID { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public DateTime LastModification { get; set; }
    }
}
