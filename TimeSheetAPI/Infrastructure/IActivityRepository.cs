using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Infrastructure
{
    interface IActivityRepository
    {
        Task<Models.Activity> GetActivity(string Id);
        
    }
}
