﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Models
{
    public class DefaultWorkweek
    {
        public DefaultWorkweek()
        {
            this.Monday = new WorkDay { Start = new DateTime(1, 1, 1, 9, 0, 0), Stop = new DateTime(1, 1, 1, 17, 0, 0) };
            this.Tuesday = new WorkDay { Start = new DateTime(1, 1, 1, 9, 0, 0), Stop = new DateTime(1, 1, 1, 17, 0, 0) };
            this.Wednesday = new WorkDay { Start = new DateTime(1, 1, 1, 9, 0, 0), Stop = new DateTime(1, 1, 1, 17, 0, 0) };
            this.Thursday = new WorkDay { Start = new DateTime(1, 1, 1, 9, 0, 0), Stop = new DateTime(1, 1, 1, 17, 0, 0) };
            this.Friday = new WorkDay { Start = new DateTime(1, 1, 1, 9, 0, 0), Stop = new DateTime(1, 1, 1, 17, 0, 0) };
            this.Saturday = new WorkDay { Start = new DateTime(1, 1, 1, 9, 0, 0), Stop = new DateTime(1, 1, 1, 17, 0, 0) };
            this.Sunday = new WorkDay { Start = new DateTime(1, 1, 1, 9, 0, 0), Stop = new DateTime(1, 1, 1, 17, 0, 0) };
        }
        [Key]
        public string Id { get; set; }
        public virtual WorkDay Monday { get; set; }
        public virtual WorkDay Tuesday { get; set; }
        public virtual WorkDay Wednesday { get; set; }
        public virtual WorkDay Thursday { get; set; }
        public virtual WorkDay Friday { get; set; }
        public virtual WorkDay Saturday { get; set; }
        public virtual WorkDay Sunday { get; set; }
    }
}