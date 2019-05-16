using System;
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
        public WorkDay Monday { get; set; }
        public WorkDay Tuesday { get; set; }
        public WorkDay Wednesday { get; set; }
        public WorkDay Thursday { get; set; }
        public WorkDay Friday { get; set; }
        public WorkDay Saturday { get; set; }
        public WorkDay Sunday { get; set; }
    }
}