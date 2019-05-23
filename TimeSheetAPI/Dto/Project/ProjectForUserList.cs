using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Dto
{
    public class ProjectForUserList
    {
        public string ProjectId { get; set; }
        public List<string> UserIds { get; set; }
    }
}
