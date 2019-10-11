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
using Microsoft.Extensions.Logging;

namespace AlarmRegistrationSystem.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class AdminController : Controller
    {
        private IMachineRepository repository;
        private int pageSize = 10;
        private ILogger<AdminController> logger;
        private ISytemElements systemElements;
        
        private void ErrorAlert(Exception ex, string errorText, string logErrorText)
        {
            TempData["Error"] = errorText;
            logger.LogError(ex + " || " + logErrorText);        
        }

        private ListViewModel<Machine> RepositoryFilter(string state, string searchText, string currentPage)
        {
            IQueryable<Machine> repo;
            try
            {
                repo = repository.Machines;
            }
            catch(Exception ex)
            {
                throw ex;
            }

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
            ListViewModel <Machine>viewModel = new ListViewModel<Machine>()
            {
                Objects = repo,
                PagingInfo = PageModel
            };
            return viewModel;
        }

        public AdminController(IMachineRepository repository, ILogger<AdminController> logger, ISytemElements systemElements)
        {
            this.repository = repository;
            this.logger = logger;
            this.systemElements = systemElements;
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
                ErrorAlert(ex, systemElements.ErrorMessages["database"], "Unable to List Machines because of RepositoryFilter (database) Exception.");
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
                ErrorAlert(ex, systemElements.ErrorMessages["database"], "Unable to Show Edit Machine View because of FirstOrDefault from database Exception");
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
                ErrorAlert(ex, systemElements.ErrorMessages["database"], "Unable to Show Edit Machine View because of FirstOrDefault from database Exception");
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
                    ErrorAlert(ex, systemElements.ErrorMessages["database"], "Unable to Edit Machine because FirstOrDefault / SaveMachine (database) Exception");
                }

                if(value)
                {
                    if (tmpMachine == null)
                    {
                        TempData["Message"] = "Maszyna została dodana";
                    }
                    else
                    {
                        TempData["Message"] = "Maszyna została edytowana";
                    }
                }
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
            Machine machine = null;
            try
            {
                machine = repository.DeleteMachine(uniqueId);
            }
            catch(Exception ex)
            {
                machine = null;
                ErrorAlert(ex, systemElements.ErrorMessages["database"], "Unable to Delete Machine because of DeleteMachine (database) Exception");
            }
            if(machine != null)
            {
                TempData["Message"] = "Maszyna zostala usunieta";
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
                ErrorAlert(ex, systemElements.ErrorMessages["database"], "Unable to Delete Machine, beacuse of RepositoryFilter (database) Exception");
            }
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
            Machine tmpMachine = null;
            try
            {
                tmpMachine = repository.Machines.FirstOrDefault(m => m.MachineUniqueId == model.Machine.MachineUniqueId);
            }
            catch(Exception ex)
            {
                ErrorAlert(ex, systemElements.ErrorMessages["database"], "Unable to VerifyID because of FirstOrDefault (database) Exception");
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