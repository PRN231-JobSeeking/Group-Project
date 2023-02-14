﻿using AppCore;
using AppCore.Models;
using AppRepository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRepository.Repositories.Implement
{
    internal class LevelRepository : GenericRepository<Level>, ILevelRepository
    {
        public LevelRepository(Context context) : base(context)
        {
        }
    }
}
