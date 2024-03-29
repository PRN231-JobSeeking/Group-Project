﻿using ClientRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Service
{
    public interface IInterviewService : IBaseService<InterviewModel>
    {
        Task<IEnumerable<AccountModel>?> GetAvailableInterviewers(int slotId, DateOnly date, int applicationId, string? token);        
    }
}
