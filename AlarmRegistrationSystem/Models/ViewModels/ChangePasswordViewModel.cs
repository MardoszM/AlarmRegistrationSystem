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

        [Required(ErrorMessage = "Proszę wprowadzic obecne haslo")]
        [StringLength(maximumLength: 15 ,MinimumLength = 8, ErrorMessage = "Haslo musi zawierać pomiedzy 8 a 15 znaków")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Proszę wprowadzic obecne haslo ponownie")]
        [Compare("OldPassword",ErrorMessage = "Podane Haslo nie jest takie same jak poprzednie")]
        public string SecondOldPassword { get; set; }

        [Required(ErrorMessage = "Proszę wprowadzic nowe haslo")]
        //[StringLength(maximumLength: 15 , MinimumLength = 8, ErrorMessage = "Nowe haslo musi zawierać od 8 do 15 znaków")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[#$^+=!*()@%&]).{8,15}", ErrorMessage = "Hasło musi składać się z 8 do 15 znaków oraz zawierać: małą, dużą literę, liczbę z zakresu [0-9] oraz znak z zakresu: \"#$^+=!*()@%&\"")]
        public string NewPassword { get; set; }
    }
}
