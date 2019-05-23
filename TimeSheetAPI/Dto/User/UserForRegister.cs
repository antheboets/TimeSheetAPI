using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Dto
{
    public class UserForRegister
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public double Sal { get; set; }
    }
}
