﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Infrastructure
{
    interface IProjectRepository
    {
        Task<bool> Create(Models.Project project);
        Task<Models.Project> Get(Models.Project project);
    }
}
