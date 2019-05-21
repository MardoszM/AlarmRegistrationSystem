using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlarmRegistrationSystem.Infrastructure;
using AlarmRegistrationSystem.Models;
using AlarmRegistrationSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AlarmRegistrationSystem.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private SignInManager<AppUser> signInManager;
        private IConfiguration Configuration { get; }
        private IQueryable<AppUser> repository;

        public AccountController(UserManager<AppUser> userMgr, RoleManager<IdentityRole> roleMgr, SignInManager<AppUser> signInMgr, IConfiguration configuration)
        {
            userManager = userMgr;
            roleManager = roleMgr;
            signInManager = signInMgr;
            Configuration = configuration;
            repository = userManager.Users;
        }

        private void TranslateRole(string role)
        {
            switch(role)
            {
                case "Employes":
                    role = "Pracownicy";
                        break;
                case "Mechanics":
                    role = "Mechanicy";
                    break;
                case "Administrators":
                    role = "Administratorzy";
                    break;
            }
        }

        [Authorize]
        public async Task<IActionResult> ListUsers()
        {
            UserListViewModel userInfo = new UserListViewModel();
            ListViewModel<UserListViewModel> model = new ListViewModel<UserListViewModel>();
            List<UserListViewModel> tmpRepo = new List<UserListViewModel>();
            foreach(var user in repository)
            {
                var result = await userManager.GetRolesAsync(user);
                string role = result[0];
                TranslateRole(role);
                tmpRepo.Add(new UserListViewModel() { User = user, Role = role });
            }
            model.Objects = tmpRepo;
            return View("List", model);
        }

        [Authorize]
        public IActionResult CreateAccount() => View();

        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateUserViewModel model)
        {
            if (model != null)
            {
                AppUser user = new AppUser
                {
                    FirstName = model.FirstName,
                    SecondName = model.SecondName,
                    Email = model.Email,
                    UserName = model.UserName
                };
                string password = GenerateRandomPassword(new CustomizedPasswordOptions());
                string path = Configuration["Data:UserDataFile:Path"];
                string fileName = Configuration["Data:UserDataFile:FileName"];
                user.SaveToExcel(password,fileName,path);
                IdentityResult result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    if (await roleManager.FindByNameAsync(model.Role) != null)
                    {
                        await userManager.AddToRoleAsync(user, model.Role);
                    }
                }
            }
            return RedirectToAction(actionName: "Index", controllerName: "Admin");
        }

        public IActionResult Login(string returnUrl)
        {
            if(User.Identity.IsAuthenticated)
            {
                return NotFound();
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                List<Func<string,Task<AppUser>>> list = new List<Func<string,Task<AppUser>>>();
                list.Add(userManager.FindByNameAsync);
                list.Add(userManager.FindByEmailAsync);
                AppUser user = null;
                foreach(var method in list)
                {
                    user = await method(model.LoginName);
                    if(user != null)
                    {
                        break;
                    }
                }

                if(user != null)
                {
                    await signInManager.SignOutAsync();
                    var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return (Redirect(model?.ReturnUrl ?? "/Home/Index"));
                    }
                }
            }
            ModelState.AddModelError("", "Nieprawidłowa nazwa użytkownika/email lub haslo");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        public async Task<string> GenerateUserName(string FirstName, string SecondName)
        {
            int counter = 1;
            bool value = false;
            string userName;
            string tmpUserName;
            userName = String.Concat(FirstName.Take(2)) + String.Concat(SecondName.Take(3));
            tmpUserName = userName;
            do
            {
                value = await VerifyUserName(tmpUserName);
                if(!value)
                {
                    tmpUserName = userName + counter++.ToString();
                }
            } while (!value);
            return tmpUserName;
        }


        public async Task<string> GenerateRandomUserName()
        {
            CustomizedPasswordOptions options = new CustomizedPasswordOptions
            {
                RequireNonAlphanumeric = false,
                RequiredUniqueChars = 0
            };
            string[] randomChars = new[] {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ",
                "abcdefghijkmnopqrstuvwxyz", 
                "0123456789" };

            string userName;
            bool value = false;
            do
            {   
                userName = GenerateRandomPassword(options,randomChars);
                value = await VerifyUserName(userName);
                
            } while (!value);

            return userName;
        }

        public string GenerateRandomPassword(CustomizedPasswordOptions opts = null, string[] randomChars = null)
        {
            if (randomChars == null)
            {
                randomChars = new[] {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
                "abcdefghijkmnopqrstuvwxyz",    // lowercase
                "0123456789",                   // digits
                "!@#$%&()"                        // non-alphanumeric
                };
            }
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

        public async Task<bool> VerifyUserName(string UserName)
        {
            IdentityUser user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> VerifyEmail(string Email)
        {
            IdentityUser user = await userManager.FindByEmailAsync(Email);
            if(user == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}