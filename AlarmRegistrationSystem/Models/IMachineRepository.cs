using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Models
{
    public interface IMachineRepository
    {
        IQueryable<Machine> Machines { get;}
        bool SaveMachine(Machine machine);
        Machine DeleteMachine(string uniqueId);
    }
}
