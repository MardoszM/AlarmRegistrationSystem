using AlarmRegistrationSystem.Controllers;
using AlarmRegistrationSystem.Hubs;
using AlarmRegistrationSystem.Models;
using AlarmRegistrationSystem.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace AlarmRegistrationSystem.Tests
{
    public class AdminControlerTests
    {
        [Theory]
        [InlineData("true","","1",6)]
        [InlineData("false", "", "1", 1)]
        [InlineData("true", "WD-40", "2", 0)]
        [InlineData("true", "A5", "1", 1)]
        public void CanListMachines(string state, string searchText, string currentPage, int count)
        {
            Mock<IMachineRepository> mock = new Mock<IMachineRepository>();
            Mock<IStringLocalizer<SharedResources>> mock1 = new Mock<IStringLocalizer<SharedResources>>();
            Mock<ILogger<AdminController>> mock2 = new Mock<ILogger<AdminController>>();
            Mock<IHubContext<ChatHub>> mock3 = new Mock<IHubContext<ChatHub>>();
            mock.Setup(m => m.Machines).Returns(new Machine[]
            {
                new Machine{Brand = "Brand1", Location = "Building A", MachineID = 1, MachineUniqueId = "A1", Model = "Model3", State = true },
                new Machine{Brand = "Brand1", Location = "Building B", MachineID = 2, MachineUniqueId = "A2", Model = "Model1", State = true },
                new Machine{Brand = "Brand1", Location = "Building C", MachineID = 3, MachineUniqueId = "A3", Model = "Model2", State = false },
                new Machine{Brand = "Brand1", Location = "Building B", MachineID = 4, MachineUniqueId = "A4", Model = "Model2", State = true },
                new Machine{Brand = "Brand2", Location = "Building A", MachineID = 5, MachineUniqueId = "A5", Model = "Model2", State = true },
                new Machine{Brand = "Brand2", Location = "Building B", MachineID = 6, MachineUniqueId = "A6", Model = "Model1", State = true },
                new Machine{Brand = "Brand3", Location = "Building C", MachineID = 7, MachineUniqueId = "A7", Model = "Model1", State = true }
            }.AsQueryable<Machine>());
            AdminController target = new AdminController(mock.Object, mock2.Object, mock1.Object, mock3.Object);
            target.ControllerContext = new ControllerContext();
            target.ControllerContext.HttpContext = new DefaultHttpContext();
            ListViewModel<Machine> result = GetViewModel<ListViewModel<Machine>>(target.ListMachines(state,searchText,currentPage));
            Assert.Equal(count, result.Objects.Count());
        }


        [Fact]
        public void CanEditMachine()
        {
            Mock<IMachineRepository> mock = new Mock<IMachineRepository>();
            Mock<IStringLocalizer<SharedResources>> mock1 = new Mock<IStringLocalizer<SharedResources>>();
            Mock<ILogger<AdminController>> mock2 = new Mock<ILogger<AdminController>>();
            Mock<IHubContext<ChatHub>> mock3 = new Mock<IHubContext<ChatHub>>();
            mock.Setup(m => m.Machines).Returns(new Machine[]
            {
                new Machine{Brand = "Brand1", Location = "Building A", MachineID = 1, MachineUniqueId = "A1", Model = "Model3", State = true },
                new Machine{Brand = "Brand1", Location = "Building B", MachineID = 2, MachineUniqueId = "A2", Model = "Model1", State = true },
                new Machine{Brand = "Brand1", Location = "Building C", MachineID = 3, MachineUniqueId = "A3", Model = "Model2", State = false },
                new Machine{Brand = "Brand1", Location = "Building B", MachineID = 4, MachineUniqueId = "A4", Model = "Model2", State = true },
                new Machine{Brand = "Brand2", Location = "Building A", MachineID = 5, MachineUniqueId = "A5", Model = "Model2", State = true },
                new Machine{Brand = "Brand2", Location = "Building B", MachineID = 6, MachineUniqueId = "A6", Model = "Model1", State = true },
                new Machine{Brand = "Brand3", Location = "Building C", MachineID = 7, MachineUniqueId = "A7", Model = "Model1", State = true }
            }.AsQueryable<Machine>());
            AdminController target = new AdminController(mock.Object, mock2.Object, mock1.Object, mock3.Object);
            EditMachineViewModel result = GetViewModel<EditMachineViewModel>(target.EditMachine(1));
            Assert.Equal(1,result.Machine.MachineID);
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}
