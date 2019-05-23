using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Dto
{
    public class UserForGet
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string RoleId { get; set; }
        public string DefaultWorkweekId { get; set; }
        public ICollection<string> ExceptionWorkDayIds { get; set; }
        public ICollection<string> LogIds { get; set; }
        public bool ChangeHistory { get; set; }
        public DateTime Month { get; set; }
    }
}
