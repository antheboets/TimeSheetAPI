﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Models
{
    public class WorkDayException
    {
        [Key]
        public string Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
