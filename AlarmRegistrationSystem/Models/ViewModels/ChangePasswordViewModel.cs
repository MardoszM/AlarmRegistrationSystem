using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string SpecifiedUserName { get; set; }

        [Required(ErrorMessage = "actualpassword")]
        [StringLength(maximumLength: 15 ,MinimumLength = 8, ErrorMessage = "passwordlength")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "actualpasswordagain")]
        [Compare("OldPassword",ErrorMessage = "passwordsdoesnotequal")]
        public string SecondOldPassword { get; set; }

        [Required(ErrorMessage = "enternewpassword")]
        //[StringLength(maximumLength: 15 , MinimumLength = 8, ErrorMessage = "Nowe haslo musi zawierać od 8 do 15 znaków")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[#$^+=!*()@%&]).{8,15}", ErrorMessage = "passwordrequirement")]
        public string NewPassword { get; set; }
    }
}
