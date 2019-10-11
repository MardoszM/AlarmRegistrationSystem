using AlarmRegistrationSystem.Infrastructure;
using AlarmRegistrationSystem.Models;
using AlarmRegistrationSystem.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Components
{
    public class MenuViewComponent :ViewComponent
    {
        private UserManager<AppUser> userManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private List<Dictionary<string, string>> links;

        public MenuViewComponent(UserManager<AppUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            this.userManager = userManager;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            AppUser user = await userManager.FindByNameAsync(User.Identity.Name);
            var roles = await userManager.GetRolesAsync(user);
            string role = roles[0];
            string path = _hostingEnvironment.ContentRootPath + "\\Infrastructure\\JsonData\\" + role + "Links.json";
            links = JsonDataReader.ReadJson<List<Dictionary<string,string>>>(path);
            MenuViewModel model = new MenuViewModel
            {
                Links = links,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                Email = user.Email
            };

            return View(model);
        }
    };
}
