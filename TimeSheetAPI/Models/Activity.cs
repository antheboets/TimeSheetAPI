using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Models
{
    public class Activity
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
