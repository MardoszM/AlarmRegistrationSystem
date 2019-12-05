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
        Active, In_repair, On_hold, Finish
    }

    public class EmergencySubassembly
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class NotificationES
    {
        public int Id { get; set; }
        public int NotificationId { get; set; }
        public int ESId { get; set; }
    }
    public class Notification
    {
        public int notificationId;

        public int NotificationID { get; set; }
        public string Declarant { get; set; }
        [Remote(action:"VerifyMachineId", controller: "Notification", ErrorMessage = "machineidtaken", AdditionalFields = nameof(MachineUniqueID))]
        [Required(ErrorMessage = "entermachineidwithfault")]
        public string MachineUniqueID { get; set; }
        public NotificationStates State { get; set; }
        [MinLength(20, ErrorMessage = "longerfaultdescription")]
        [MaxLength(1000, ErrorMessage = "shorterfaultdescription")]
        [Required(ErrorMessage = "enteraccidentdescription")]
        public string MainDescription { get; set; }
        public DateTime CreationDate { get; set; }

        public DateTime EndTime { get; set; }

    }

    public class JoiningPeriod
    {
        public int JoiningPeriodId { get; set; }

        public int NotificationId { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }

    public class Machine
    {
        public int MachineID { get; set; }

        public bool State { get; set; }

        [Remote(action: "VerifyId", controller: "Admin", ErrorMessage = "idtaken", AdditionalFields = nameof(MachineID))]
        [Required(ErrorMessage = "enteruniqueid")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "uniqueidlength")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "uniqueidchars")]
        public string MachineUniqueId { get; set; }

        [Required(ErrorMessage = "enterlocation")]
        public string Location { get; set; }

        [Required(ErrorMessage = "enterbrand")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "entermodel")]
        public string Model { get; set; }
    }

    public class Description
    {
        public int DescriptionID { get; set; }
        public int NotificationID { get; set; }
        public string Author { get; set; }

        [MinLength(20, ErrorMessage = "longerdescriptiontext")]
        [MaxLength(1000, ErrorMessage = "shorterdescriptiontext")]
        [Required(ErrorMessage = "enterdescriptiontext")]
        public string Text { get; set; }
        public DateTime LastModification { get; set; }
    }
}
