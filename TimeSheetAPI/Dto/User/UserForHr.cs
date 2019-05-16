using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Dto
{
    public class UserForHr
    {
        public string UserId { get; set; }
        public string TotalHours { get; set; }
        public int Month { get; set; }
        public double MonthSalary { get; set; }
    }
}