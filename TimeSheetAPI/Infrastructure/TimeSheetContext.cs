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
        public DbSet<Project> Project { get; set; }
        public DbSet<Activity> Activity { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<DefaultWorkweek> DefaultWorkweek { get; set; }
        public DbSet<ProjectUser> ProjectUser { get; set; }
        public DbSet<WorkMonth> WorkMonth { get; set; }
        public DbSet<WorkDayException> WorkDayException { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectUser>().HasKey(o => new { o.ProjectId, o.UserId });
            //modelBuilder.Entity<Project>().HasOptional(a => a.Logs).WithOptionalDependent().WillCascadeOnDelete(true);

        }
    }
}
