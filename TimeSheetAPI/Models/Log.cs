using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Models
{
    public class Log
    {
        public string Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
        public string Description { get; set; }
        public string ProjectId { get; set; }
        public Project Project { get; set; }
        public string ActivityId { get; set; }
        public Activity Activity { get; set;}
        public string UserId { get; set; }
    }
}
