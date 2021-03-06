﻿using CoronaApp.Entities;
using CoronaApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoronaApp.Services
{
    public interface IPathService
    {
        Task<List<PathModel>> GetAllPaths();
        Task<List<PathModel>> GetPathsBySearch(PathSearch locationSearch);
    }
}
