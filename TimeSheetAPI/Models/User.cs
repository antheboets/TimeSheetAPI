﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Models
{
    
    public class User
    {
        public enum Roles { CONSULTANT, MANAGER, HUMANRESOURCE}
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Byte[] PasswordHash { get; set; }
        public Byte[] PasswordSalt { get; set; }
        public Roles Role { get; set; }
        public ICollection<Log> Logs { get; set; }
    }
}
