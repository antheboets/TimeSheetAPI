using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Dto
{
    public class UserForUpdate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string RoleId { get; set; }
        public string DefaultWorkweekId { get; set; }
        public bool ChangeHistory { get; set; }
    }
}
