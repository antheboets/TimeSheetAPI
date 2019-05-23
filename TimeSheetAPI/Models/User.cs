using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Models
{

    public class User
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Byte[] PasswordHash { get; set; }
        public Byte[] PasswordSalt { get; set; }
        public string RoleId { get; set; }
        public Role Role { get; set; }
        public string DefaultWorkweekId { get; set; }
        public DefaultWorkweek DefaultWorkweek { get; set; }
        public ICollection<WorkDayException> ExceptionWorkDays { get; set; }
        public ICollection<Log> Logs { get; set; }
        public List<ProjectUser> Projects { get; set; }
        public bool ChangeHistory { get; set; }
        public double HourlyRate { get; set; }
    }
}
