﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Models
{
    public class Project
    {
        public string Id { get; set; }
        public string Name{get; set;}
        public ICollection<Activity> Activitys { get; set; }
    }
}
