using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlarmRegistrationSystem.Infrastructure;
using AlarmRegistrationSystem.Models;
using AlarmRegistrationSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AlarmRegistrationSystem.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private SignInManager<AppUser> signInManager;
        private IConfiguration Configuration { get; }
        private IQueryable<AppUser> repository;
        private IHostingEnvironment hostingEnvironment;
        private List<string> roles = null;
        private int pageSize = 10;

        public AccountController(UserManager<AppUser> userMgr, RoleManager<IdentityRole> roleMgr, SignInManager<AppUser> signInMgr, IConfiguration configuration, IHostingEnvironment _hostingEnvironment)
        {
            userManager = userMgr;
            roleManager = roleMgr;
            signInManager = signInMgr;
            Configuration = configuration;
            repository = userManager.Users;
            hostingEnvironment = _hostingEnvironment;
        }

        public static string TranslateRole(string role)
        {
            switch (role)
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
            return role;
        }

        public static string GetControllerFromPath(string path)
        {
            string controller = path.Split('/')[0];
            return controller;
        }

        public static string GetActionFromPath(string path)
        {
            string action = path.Split('/').Last();
            return action;
        }


        private List<string> GetRoles()
        {
            string path = hostingEnvironment.ContentRootPath + "\\Infrastructure\\JsonData\\roles.json";
            var roles = JsonDataReader.ReadJson<List<string>>(path);
            return roles;
        }

        private async Task<ListViewModel<UserListViewModel>> RepositoryFilter(string searchRole,string searchText, string currentPage)
        {
            ListViewModel<UserListViewModel> model = new ListViewModel<UserListViewModel>();
            List<UserListViewModel> tmpRepo = new List<UserListViewModel>();
            int currPage;

            foreach (var user in repository)
            {
                var result = await userManager.GetRolesAsync(user);
                string role = result[0];
                role = TranslateRole(role);
                tmpRepo.Add(new UserListViewModel() { User = user, Role = role, Id = user.Id });
            }

            if (!Int32.TryParse(currentPage, out currPage))
            {
                currPage = 1;
            }

            if(searchText != null)
            {
                tmpRepo = tmpRepo
                    .Where(u =>
                (u.User.FirstName + u.User.SecondName).IsStringContains(searchText.Replace(" ", "")) ||
                u.User.UserName.IsStringContains(searchText) ||
                u.User.Email.IsStringContains(searchText)
                ).ToList();
                currPage = 1;
            }

            searchRole = AccountController.TranslateRole(searchRole);
            if (searchRole != "" && searchRole != null)
            {
                tmpRepo = tmpRepo.Where(u => u.Role == searchRole).ToList();
                currPage = 1;
            }

            model.PagingInfo = new PagingInfo() { CurrentPage = currPage, ItemsPerPage = pageSize, TotalItems = tmpRepo.Count() };
            model.Objects = tmpRepo
                .OrderBy(m => m.Id)
                .Skip((currPage - 1) * pageSize)
                .Take(pageSize);
            return model;
        }

        [Authorize]
        public async Task<IActionResult> ListUsers(string searchRole,string searchText,string currentPage)
        {
            ListViewModel<UserListViewModel> model = await RepositoryFilter(searchRole,searchText,currentPage);
            
            if (Request.IsAjaxRequest())
            {
                return View("Partial/_UsersTable", model);
            }
            else
            {
                return View("List", model);
            }
            }

        [Authorize]
        public ViewResult ChangePasswordAbsolutely(string userName)
        {
            ChangePasswordViewModel model = new ChangePasswordViewModel() { SpecifiedUserName = userName };
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePasswordAbsolutely(string SpecifiedUserName, string NewPassword)
        {
            AppUser user = await userManager.FindByNameAsync(SpecifiedUserName);
            IdentityResult result = await userManager.RemovePasswordAsync(user);
            IdentityResult result2 = await userManager.AddPasswordAsync(user, NewPassword);
            if(!result.Succeeded || !result2.Succeeded)
            {
                TempData["Message"] = "Zmiana hasla zakonczona niepowodzeniem. Wystapil blad"; 
            }
            else
            {
                TempData["Message"] = "Haslo uzytkownika " + SpecifiedUserName + " zostalo zmienione]";
            }
            return RedirectToAction("ListUsers");
        }

        [Authorize]
        public ViewResult ChangePassword(string userName)
        {
            ChangePasswordViewModel model = new ChangePasswordViewModel() { SpecifiedUserName = userName };
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string SpecifiedUserName, string OldPassword, string NewPassword)
        {
            
            AppUser user = await userManager.FindByNameAsync(SpecifiedUserName);
            IdentityResult result = await userManager.ChangePasswordAsync(user, OldPassword, NewPassword);
            if(result.Succeeded)
            {
                TempData["Message"] = "Haslo uzytkownika " + SpecifiedUserName + " zostalo zmienione";
            }
            else
            {
                TempData["Message"] = "Zmiana hasla nie zostala ukonczona. Wystapil blad.";
            }
            return RedirectToAction("ListUsers");
        }

        [Authorize]
        public async Task<IActionResult> DeleteUser(string uniqueUserName, string searchRole, string searchText, string currentPage)
        {
            AppUser user = await userManager.FindByNameAsync(uniqueUserName);
            await userManager.DeleteAsync(user);
            ListViewModel<Models.ViewModels.UserListViewModel> model = await RepositoryFilter(searchRole, searchText, currentPage);
            return View("Partial/_UsersTable", model);
        }

        [Authorize]
        public IActionResult CreateAccount(string returnurl = "Admin/Index")
        {
            string controller = GetControllerFromPath(returnurl);
            string action = GetActionFromPath(returnurl);
            if(roles == null)
            {
                roles = GetRoles();
            }
            CreateUserViewModel model = new CreateUserViewModel()
            {
                roles = roles,
                Controller = controller,
                Action = action
            };
            return View(model);
        }

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
            return RedirectToAction(nameof(ListUsers));
        }

        [AllowAnonymous]
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
            ModelState.AddModelError("Error", "Nieprawidłowa nazwa użytkownika/email lub haslo");
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
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