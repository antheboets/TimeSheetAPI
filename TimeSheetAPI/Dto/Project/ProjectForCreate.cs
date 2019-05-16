using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Dto
{
    public class ProjectForCreate
    {
        public string Name { get; set; }
        public ICollection<Dto.UserId> UsersOnTheProject { get; set; }
        public string CompanyId { get; set; }
        public bool InProgress { get; set; }
        public bool Overtime { get; set; }
        public bool Billable { get; set; }
    }
}
