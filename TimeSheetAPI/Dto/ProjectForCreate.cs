﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Dto
{
    public class ProjectForCreate
    {
        public string Name { get; set; }
        public ICollection<User> UsersOnTheProject { get; set; }
        public string CompanyId { get; set; }
        public bool Overtime { get; set; }
        public bool Billable { get; set; }
    }
}
