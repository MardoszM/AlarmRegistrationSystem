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

        public List<Dictionary<string, string>> ReadFromJson(string path)
        {
            List<Dictionary<string, string>> tmpList;
            string jsonData = File.ReadAllText(path);
            tmpList = JsonConvert.DeserializeObject(jsonData, typeof(List<Dictionary<string, string>>)) 
                as List<Dictionary<string, string>>;
            return tmpList;
        }

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
            string path = _hostingEnvironment.ContentRootPath + "\\wwwroot\\JsonData\\" + role + "Links.json";
            links = ReadFromJson(path);
            MenuViewModel model = new MenuViewModel
            {
                Links = links,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                Email = user.Email
            };

            return View(model:model);
        }
    };
}
