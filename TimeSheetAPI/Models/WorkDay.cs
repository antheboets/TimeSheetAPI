using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Models
{
    public class WorkDay
    {
        public string Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
    }
}
