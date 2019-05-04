using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Dto
{
    public class Project
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<string> ActivitysId { get; set; }
        public ICollection<string> UsersId { get; set; }
        public ICollection<string> LogsId { get; set; }
        public string CompanyId { get; set; }
        public bool Overtime { get; set; }
        public bool Billable { get; set; }
    }
}
