using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Models.ViewModels
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Proszę wprowadzić imię.")]
        [StringLength(maximumLength: 30, ErrorMessage = "Imie musi zawierac co najmniej 2 znaki", MinimumLength = 2)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Imie może zawierać tylko litery")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Proszę wprowadzić nazwisko.")]
        [StringLength(maximumLength: 30, ErrorMessage = "Nazwisko musi zawierac co najmniej 3 znaki", MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Nazwisko może zawierać tylko litery")]
        public string SecondName { get; set; }
        [Required(ErrorMessage = "Proszę wporwadzić nazwę użytkownika")]
        
        [Remote(action: "VerifyUserName", controller: "Account", ErrorMessage = "Podana nazwa użytkownika jest już zajęta")]
        [StringLength(maximumLength:15, ErrorMessage = "Nazwa użytkownika musi zawierać od 5 do 15 znaków", MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Nazwa Uzytkownika może zawierać tylko litery i cyfry")]
        public string UserName { get; set; }

        [Remote(action: "VerifyEmail", controller: "Account", ErrorMessage = "Podany adres email jest już zajęty")]
        [Required(ErrorMessage = "Proszę wprowadzić email")]
        [EmailAddress(ErrorMessage = "Wprowadzony email nie jest poprawny")]
        public string Email { get; set; }
        public string Role { get; set; }

    }
}
