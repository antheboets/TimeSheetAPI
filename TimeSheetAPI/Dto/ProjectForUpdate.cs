using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Dto
{
    public class ProjectForUpdate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<Models.Activity> Activitys { get; set; }
        public string CompanyId { get; set; }
    }
}
