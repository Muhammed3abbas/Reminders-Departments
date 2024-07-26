using Microsoft.EntityFrameworkCore;
using RingoMediaReminder.Entities;
using System.Collections.Generic;

namespace RingoMediaReminder.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
    }
}
