using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Models
{
    public class CreateUserModel
    {
        [Remote(action: "VerifyUserName", controller: "Admin", ErrorMessage = "Taka nazwa użytkownika jest już zajęta")]
        [Required(ErrorMessage = "Proszę wprowadzić unikalną nazwę użytkownika")]
        [StringLength(6, ErrorMessage = "Nazwa użytkownika musi zawierać conajmniej 6 znaków")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Proszę wprowadzić hasło")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%&()]).{8,}$", ErrorMessage = "Hasło musi składać się" +
            " z conajmniej 6 znaków oraz zawierać conajmniej jedną: cyfrę, Wielką i małą literę oraz znak specjalny [!@#$%&()]")]
        public string Password { get; set; }

        public string Role { get; set; }

        public bool WasGenerated { get; set; }
    }
}
