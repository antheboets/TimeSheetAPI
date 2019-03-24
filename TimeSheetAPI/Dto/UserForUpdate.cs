using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Dto
{
    public class UserForUpdate
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Models.User.Roles Role { get; set; }
    }
}
