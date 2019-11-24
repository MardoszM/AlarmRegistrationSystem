using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlarmRegistrationSystem.Controllers.SystemFunctionality;
using AlarmRegistrationSystem.Hubs;
using AlarmRegistrationSystem.Infrastructure;
using AlarmRegistrationSystem.Models;
using AlarmRegistrationSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace AlarmRegistrationSystem.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class AdminController : BasicController
    {
        private IMachineRepository repository;
        private int pageSize = 10;

        public AdminController(IMachineRepository repository, ILogger<AdminController> logger, IStringLocalizer<SharedResources> localizer, IHubContext<ChatHub> connector) :base(connector, localizer, logger)
        {
            this.repository = repository;
        }

        private ListViewModel<Machine> RepositoryFilter(string state, string searchText, string currentPage)
        {
            IQueryable<Machine> repo;
            PagingInfo PageModel;
            try
            {
                repo = repository.Machines;

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

            PageModel = new PagingInfo()
            {
                CurrentPage = currPage,
                ItemsPerPage = pageSize,
                TotalItems = repo.Count()
            };

            repo = repo
                .OrderBy(m => m.MachineID)
                .Skip((currPage - 1) * pageSize)
                .Take(pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            ListViewModel <Machine>viewModel = new ListViewModel<Machine>()
            {
                Objects = repo,
                PagingInfo = PageModel
            };
            return viewModel;
        }

        public IActionResult ListMachines(string state, string searchText, string currentPage)
        {
            ListViewModel<Machine> viewModel;
            try
            {
                viewModel = RepositoryFilter(state, searchText, currentPage);
            }
            catch(Exception ex)
            {
                
                viewModel = new ListViewModel<Machine>();
                viewModel.PagingInfo = new PagingInfo()
                {
                    CurrentPage = 1,
                    ItemsPerPage = 1,
                    TotalItems = 0
                };
                viewModel.Objects = null;
                ErrorAlert(ex, localizer["database"], "Unable to List Machines because of RepositoryFilter (database) Exception.");
            }

            if (Request.IsAjaxRequest())
            {
                return View("Partial/_MachinesTable", viewModel);
            }
            return View("List", viewModel);
        }

        public ViewResult EditMachine(int machineId)
        {
            Machine machine;
            try
            {
                machine = repository.Machines.FirstOrDefault(m => m.MachineID == machineId);
            }
            catch (Exception ex)
            {
                ErrorAlert(ex, localizer["database"], "Unable to Show Edit Machine View because of FirstOrDefault from database Exception");
                return View("List", new ListViewModel<Machine>() { 
                PagingInfo = new PagingInfo ()
                });
            }
            EditMachineViewModel viewModel = new EditMachineViewModel();
            try
            {
                viewModel.Machine = repository.Machines.FirstOrDefault(m => m.MachineID == machineId);
            }
            catch(Exception ex)
            {
                ErrorAlert(ex, localizer["database"], "Unable to Show Edit Machine View because of FirstOrDefault from database Exception");
                return View("List", new ListViewModel<Machine>()
                {
                    PagingInfo = new PagingInfo { CurrentPage = 1, ItemsPerPage = 1, TotalItems = 0 }
                });
            }
            viewModel.ReturnUrl = nameof(ListMachines);
            viewModel.NewMachine = false;

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditMachine(EditMachineViewModel model)
        {
            if(ModelState.IsValid)
            {
                Machine tmpMachine = null;
                bool value = false;
                try
                {
                    tmpMachine = repository.Machines.FirstOrDefault(m => m.MachineID == model.Machine.MachineID);
                    value = repository.SaveMachine(model.Machine);
                }
                catch(Exception ex)
                {
                    value = false;
                    ErrorAlert(ex, localizer["database"], "Unable to Edit Machine because FirstOrDefault / SaveMachine (database) Exception");
                }
                return RedirectToAction(model.ReturnUrl.GetActionFromPath(), model.ReturnUrl.GetControllerFromPath());
                }
            else
            {
                return View("EditMachine", model);
            }
        }

        [HttpPost]
        public IActionResult DeleteMachine(string searchText, string state, string uniqueId, string currentPage)
        {
            Machine machine = null;
            try
            {
                machine = repository.DeleteMachine(uniqueId);
            }
            catch(Exception ex)
            {
                machine = null;
                ErrorAlert(ex, localizer["database"], "Unable to Delete Machine because of DeleteMachine (database) Exception");
            }
            if(machine != null)
            {
                SendMessageToCaller(localizer["machinedeleted"]);
            }
            ListViewModel<Machine> viewModel = null;
            try
            {
                viewModel = RepositoryFilter(state, searchText, currentPage);
            }
            catch(Exception ex)
            {
                viewModel = new ListViewModel<Machine>();
                viewModel.PagingInfo = new PagingInfo();
                viewModel.Objects = null;
                ErrorAlert(ex, localizer["database"], "Unable to Delete Machine, beacuse of RepositoryFilter (database) Exception");
            }
            return View("Partial/_MachinesTable", viewModel);
        }

        public IActionResult CreateMachine(string returnUrl = "Home/Index") => View("EditMachine", new EditMachineViewModel() {
        Machine = new Machine(),
        ReturnUrl = returnUrl,
        NewMachine = true
        });

        public bool VerifyId(EditMachineViewModel model)
       {
            Machine tmpMachine = null;
            try
            {
                tmpMachine = repository.Machines.FirstOrDefault(m => m.MachineUniqueId == model.Machine.MachineUniqueId);
            }
            catch(Exception ex)
            {
                ErrorAlert(ex, localizer["database"], "Unable to VerifyID because of FirstOrDefault (database) Exception");
            }
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