using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Models.ViewModels
{
    public class ListMachinesViewModel
    {
        public IEnumerable<Machine> Machines { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
