using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Dto
{
    public class WorkMonth
    {
        public string Id { get; set; }
        public int Month { get; set; }
        public bool Accepted { get; set; }
        public string UserId { get; set; }
        public string TotalHours { get; set; }
        public string Salary { get; set; }
    }
}
