using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlarmRegistrationSystem.Infrastructure;
using AlarmRegistrationSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlarmRegistrationSystem.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;
        private RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<AppUser> userMgr, RoleManager<IdentityRole> roleMgr)
        {
            userManager = userMgr;
            roleManager = roleMgr;
        }

        public IActionResult CreateAccount() => View();

        public IActionResult ShowUserData(string userName) => View(userManager.FindByNameAsync(userName));

        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateUserModel model)
        {
            if (model != null)
            {
                AppUser user = new AppUser
                {
                    UserName = model.UserName
                };

                IdentityResult result = await userManager.CreateAsync(user, model.Password);
                if(result != null)
                {
                    if(roleManager.FindByNameAsync(model.Role) != null)
                    {
                       await userManager.AddToRoleAsync(user, model.Role);
                    }
                }
                if (model.WasGenerated)
                {
                    return ShowUserData(user.UserName);
                }
            }
            return RedirectToAction(actionName: "Index", controllerName: "Admin");
        }

        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        public string GenerateRandomPassword(CustomizedPasswordOptions opts = null)
        {

            string[] randomChars = new[] {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
                "abcdefghijkmnopqrstuvwxyz",    // lowercase
                "0123456789",                   // digits
                "!@#$%&()"                        // non-alphanumeric
    };
            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }
    }
}