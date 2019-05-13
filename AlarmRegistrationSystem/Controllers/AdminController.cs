using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlarmRegistrationSystem.Infrastructure;
using AlarmRegistrationSystem.Models;
using AlarmRegistrationSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;

namespace AlarmRegistrationSystem.Controllers
{
    public class AdminController : Controller
    {
        private IMachineRepository repository;
        private int pageSize = 10;

        private ListMachinesViewModel RepositoryFilter(string state, string searchText, string currentPage)
        {
            IEnumerable<Machine> repo = repository.Machines as IEnumerable<Machine>;

            int currPage;
            if (!Int32.TryParse(currentPage, out currPage))
            {
                currPage = 1;
            }

            if (searchText != null)
            {
                repo = repo.Where(m =>
                m.Brand.IsStringContains(searchText) ||
                m.Location.IsStringContains(searchText) ||
                m.MachineUniqueId.IsStringContains(searchText) ||
                m.Model.IsStringContains(searchText));
                currPage = 1;
            }

            if (state != "" && state != null)
            {
                state = char.ToUpper(state[0]) + state.Substring(1);
                bool value = Boolean.Parse(state);
                repo = repo.Where(m => m.State == value);
                currPage = 1;
            }

            PagingInfo PageModel = new PagingInfo()
            {
                CurrentPage = currPage,
                ItemsPerPage = pageSize,
                TotalItems = repo.Count()
            };

            repo = repo
                .OrderBy(m => m.MachineID)
                .Skip((currPage - 1) * pageSize)
                .Take(pageSize);
            ListMachinesViewModel viewModel = new ListMachinesViewModel()
            {
                Machines = repo,
                PagingInfo = PageModel
            };
            return viewModel;
        }

        public AdminController(IMachineRepository repository) => this.repository = repository;

        public IActionResult ListMachines(string state, string searchText, string currentPage)
        {
            ListMachinesViewModel viewModel = RepositoryFilter(state, searchText, currentPage);

            if (Request.IsAjaxRequest())
            {
                return View("Partial/_MachinesTable", viewModel);
            }

            return View("List", viewModel);
        }

        public ViewResult EditMachine(int machineId) => View(new EditMachineViewModel() {
            Machine = repository.Machines.FirstOrDefault(m => m.MachineID == machineId),
            ReturnUrl = nameof(ListMachines),
            NewMachine = false
        });

        [HttpPost]
        public IActionResult EditMachine(EditMachineViewModel model)
        {
            if(ModelState.IsValid)
            {
                Machine tmpMachine = repository.Machines.FirstOrDefault(m => m.MachineID == model.Machine.MachineID);
                if(tmpMachine == null)
                {
                    TempData["Message"] = "Maszyna została dodana";
                }
                else
                {
                    TempData["Message"] = "Maszyna została edytowana";
                }
                repository.SaveMachine(model.Machine);
                return Redirect(model.ReturnUrl);
            }
            else
            {
                return View("EditMachine", model);
            }
        }

        [HttpPost]
        public IActionResult DeleteMachine(string searchText, string state, string uniqueId, string currentPage)
        {
            repository.DeleteMachine(uniqueId);
            ListMachinesViewModel viewModel = RepositoryFilter(state, searchText, currentPage);
            return View("Partial/_MachinesTable", viewModel);
        }

        public IActionResult CreateMachine(string returnUrl = nameof(Index)) => View("EditMachine", new EditMachineViewModel() {
        Machine = new Machine(),
        ReturnUrl = returnUrl,
        NewMachine = true
        });

        public IActionResult Index() => View();

        public bool VerifyId(EditMachineViewModel model)
       {
            Machine tmpMachine = repository.Machines.FirstOrDefault(m => m.MachineUniqueId == model.Machine.MachineUniqueId);
            if (model.Machine.MachineID == 0)
            {
                if (tmpMachine != null)
                {
                    return false;
                }
            }
            else
            {
                if(tmpMachine != null && model.Machine.MachineID != tmpMachine?.MachineID)
                {
                        return false;
                }
            }
            return true;
        }
    }
}