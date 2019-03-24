using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Dto
{
    public class LogForCreate
    {
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
        public String Description { get; set; }
    }
}
