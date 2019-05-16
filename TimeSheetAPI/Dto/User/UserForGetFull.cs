using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Dto
{
    public class UserForGetFull
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public DefaultWorkweek DefaultWorkweek { get; set; }
        public ICollection<WorkDayException> ExceptionWorkDays { get; set; }
        public ICollection<Log> Logs { get; set; }
        public bool ChangeHistory { get; set; }
    }
}
