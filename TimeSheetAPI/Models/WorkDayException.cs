using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Models
{
    public class WorkDayException
    {
        public string Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
    }
}
