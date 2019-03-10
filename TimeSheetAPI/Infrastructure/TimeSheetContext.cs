using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSheetAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace TimeSheetAPI.Infrastructure
{
    public class TimeSheetContext : DbContext
    {
        public TimeSheetContext(DbContextOptions<TimeSheetContext> options) : base(options) { }
        public DbSet<User> User { get; set; }
        public DbSet<Log> Log { get; set; }

    }
}
