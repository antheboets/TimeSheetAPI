using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Models
{
    public class Project
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<Activity> Activitys { get; set; }
        public List<ProjectUser> Users {get; set;}
        public ICollection<Log> Logs { get; set; }
        public string CompanyId { get; set; }
        public Company Company { get; set; }
        public bool InProgress { get; set; }
        public bool Overtime { get; set; }
        public bool Billable { get; set; }
    }
}
