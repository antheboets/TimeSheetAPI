using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Models
{
    public class DefaultWorkweek
    {
        DefaultWorkweek()
        {
            this.Monday = new Day { Start = new DateTime(0, 0, 0, 9, 0, 0), Stop = new DateTime(0, 0, 0, 9, 0, 0) };
            this.Tuesday = new Day { Start = new DateTime(0, 0, 0, 9, 0, 0), Stop = new DateTime(0, 0, 0, 9, 0, 0) };
            this.Wednesday = new Day { Start = new DateTime(0, 0, 0, 9, 0, 0), Stop = new DateTime(0, 0, 0, 9, 0, 0) };
            this.Thursday = new Day { Start = new DateTime(0, 0, 0, 9, 0, 0), Stop = new DateTime(0, 0, 0, 9, 0, 0) };
            this.Friday = new Day { Start = new DateTime(0, 0, 0, 9, 0, 0), Stop = new DateTime(0, 0, 0, 9, 0, 0) };
            this.Saturday = new Day { Start = new DateTime(0, 0, 0, 9, 0, 0), Stop = new DateTime(0, 0, 0, 9, 0, 0) };
            this.Sunday = new Day { Start = new DateTime(0, 0, 0, 9, 0, 0), Stop = new DateTime(0, 0, 0, 9, 0, 0) };
        }
        public string Id { get; set; }
        public Day Monday { get; set; }
        public Day Tuesday { get; set; }
        public Day Wednesday { get; set; }
        public Day Thursday { get; set; }
        public Day Friday { get; set; }
        public Day Saturday { get; set; }
        public Day Sunday { get; set; }
    }
}