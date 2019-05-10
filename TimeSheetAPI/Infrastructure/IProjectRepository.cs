﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Infrastructure
{
    public interface IProjectRepository
    {
        Task<bool> Create(Models.Project project);
        Task<Models.Project> GetSmall(Models.Project project);
        Task<Models.Project> GetFull(Models.Project project);
        Task<bool> Delete(Models.Project project);
        Task<bool> Update(Models.Project project);
        Task<List<Models.Project>> GetAll();
    }
}