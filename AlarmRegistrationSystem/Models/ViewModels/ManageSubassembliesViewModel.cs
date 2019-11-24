using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Models.ViewModels
{
    public class ManageSubassembliesViewModel
    {
        public List<EmergencySubassembly> Subassemblies { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
