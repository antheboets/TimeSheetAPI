using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name{get; set;}
        public Dictionary<int, Activity> ActivityList { get; set; }
    }
}
