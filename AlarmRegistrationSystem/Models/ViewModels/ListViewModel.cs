using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Models.ViewModels
{
    public class ListViewModel<T>
    {
        public IEnumerable<T> Objects { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
