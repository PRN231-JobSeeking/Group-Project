﻿using AppRepository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRepository.UnitOfWork
{
    public interface IUnitOfWork
    {
        IAccountRepository AccountRepository { get; }
        IApplicationRepository ApplicationRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IInterviewRepository InterviewRepository { get; }
        ILevelRepository LevelRepository { get; }
        ILocationRepository LocationRepository { get; }
        IPostRepository PostRepository { get; }
        IPostSkillRepository PostSkillRepository { get; }
        IRoleRepository RoleRepository { get; }
        ISkillRepository SkillRepository { get; }
        ISlotRepository SlotRepository { get; }
        IUserSkillRepository UserSkillRepository { get; }        
    }
}
