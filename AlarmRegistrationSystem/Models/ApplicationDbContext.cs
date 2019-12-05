using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Description> Descriptions { get; set; }
        public DbSet<EmergencySubassembly> EmergencySubassemblies { get; set; }
        public DbSet<NotificationES> NotificationEs { get; set; }
        public DbSet<JoiningPeriod> JoiningPeriods { get; set; }
    }
}
