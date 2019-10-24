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
        [Required(ErrorMessage = "entername")]
        [StringLength(maximumLength: 30, ErrorMessage = "namelength", MinimumLength = 2)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "namechars")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "enterlastname")]
        [StringLength(maximumLength: 30, ErrorMessage = "lastnamelength", MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "lastnamechars")]
        public string SecondName { get; set; }
        [Required(ErrorMessage = "enterusername")]
        
        [Remote(action: "VerifyUserName", controller: "Account", ErrorMessage = "usernametaken")]
        [StringLength(maximumLength:15, ErrorMessage = "usernamelength", MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "usernamechars")]
        public string UserName { get; set; }

        [Remote(action: "VerifyEmail", controller: "Account", ErrorMessage = "emailtaken")]
        [Required(ErrorMessage = "enteremail")]
        [EmailAddress(ErrorMessage = "wrongemail")]
        public string Email { get; set; }

        public string Role { get; set; }

        public List<string> roles;

        public string Controller { get; set; }

        public string Action { get; set; }
    }
}
