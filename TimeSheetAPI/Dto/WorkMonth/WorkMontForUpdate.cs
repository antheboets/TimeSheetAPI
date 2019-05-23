using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Dto
{
    public class WorkMontForUpdate
    {
        public string Id { get; set; }
        public bool Accepted { get; set; }
        public int Month { get; set; }
        public string Body { get; set; }
        public string UserId { get; set; }
    }
}
