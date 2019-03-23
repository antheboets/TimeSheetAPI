using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProjectNr { get; set; }
    }
}
