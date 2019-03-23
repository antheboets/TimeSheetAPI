using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Models
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
        public string Description { get; set; }
        public Project ProjectId { get; set; }
        public int ActivityNr { get; set;}
    }
}
