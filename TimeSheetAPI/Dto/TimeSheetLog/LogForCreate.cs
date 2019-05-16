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
        public string Description { get; set; }
        public string ProjectId { get; set; }
        public string ActivityId { get; set; }
    }
}
