using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Models
{
    public class EFMachineRepository : IMachineRepository
    {
        private ApplicationDbContext context;

        public EFMachineRepository(ApplicationDbContext ctx) => context = ctx;

        public IQueryable<Machine> Machines => context.Machines;

        public Machine DeleteMachine(string uniqueId)
        {
            Machine machine = null;
            try
            {
                machine = context.Machines.FirstOrDefault(m => m.MachineUniqueId == uniqueId);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            if (machine != null)
            {

                try
                {
                    context.Machines.Remove(machine);
                    context.SaveChanges();
                }
                catch(Exception ex)
                {
                    context.Machines.Add(machine as Machine);
                    throw ex;
                }
            }
            return machine;
        }

        public bool SaveMachine(Machine machine)
        {
            bool value = false;
            if(machine.MachineID == 0)
            {
                try
                {
                    context.Machines.Add(machine);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
                value = true;
            }
            else
            {
                Machine dbMachine = context.Machines.FirstOrDefault(m => m.MachineID == machine.MachineID);
                if(dbMachine != null)
                {
                    dbMachine.Brand = machine.Brand;
                    dbMachine.Location = machine.Location;
                    dbMachine.MachineUniqueId = machine.MachineUniqueId;
                    dbMachine.Model = machine.Model;
                    dbMachine.State = machine.State;
                    value = true;
                }
            }
            try
            {
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                context.Machines.Add(machine);
                throw ex;
            }
            return value;
        }
    }
}
