using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Dto
{
    public class Activity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ProjectId { get; set; }
    }
}
