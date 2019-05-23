using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Infrastructure
{
    public interface IActivityRepository
    {
        Task<Models.Activity> Get(Models.Activity activity);
        Task<bool> Update(Models.Activity activity);
        Task<bool> Create(Models.Activity activity);
        Task<bool> Delete(Models.Activity activity);
        Task<List<Models.Activity>> GetAll();
    }
}