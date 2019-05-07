using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Dto
{
    public class LogForUpdate
    {
        public string Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
        public String Description { get; set; }
        public string UserId { get; set; }
        public string ActivityId { get; set; }
    }
}
