using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Models
{
    public interface ISytemElements
    {
        IDictionary<string, string> ErrorMessages { get; set; }
    }
}
