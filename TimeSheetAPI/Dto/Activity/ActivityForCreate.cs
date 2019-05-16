using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Dto
{
    public class ActivityForCreate
    {
        public string Name { get; set; }
        public string ProjectId { get; set; }
    }
}
