using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Infrastructure
{
    public class CustomizedPasswordOptions :PasswordOptions
    {
        public CustomizedPasswordOptions() {
            RequiredLength = 8;
            RequiredUniqueChars = 2;
            RequireDigit = true;
            RequireLowercase = true;
            RequireNonAlphanumeric = true;
            RequireUppercase = true;
        }
    }
}
