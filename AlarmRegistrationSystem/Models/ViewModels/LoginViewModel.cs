﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "loginusername")]
        public string LoginName { get; set; }

        [Required(ErrorMessage = "password")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; } = "/";
    }
}
