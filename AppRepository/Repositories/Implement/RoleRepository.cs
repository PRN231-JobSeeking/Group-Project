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
    internal class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(Context context) : base(context)
        {
        }
    }
}
