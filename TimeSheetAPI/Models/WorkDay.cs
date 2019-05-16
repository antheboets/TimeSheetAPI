﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Models
{
    public class WorkDay
    {
        [Key]
        public string Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
    }
}
