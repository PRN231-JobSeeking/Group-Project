﻿using AppCore;
using AppCore.Models;
using AppRepository.Generic;
using AppRepository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRepository.Repositories.Implement
{
    internal class SlotRepository : GenericRepository<Slot>, ISlotRepository
    {
        public SlotRepository(Context context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
    }
}
