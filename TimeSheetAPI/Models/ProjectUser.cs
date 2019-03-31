using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Models
{
    public class ProjectUser
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public string ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
