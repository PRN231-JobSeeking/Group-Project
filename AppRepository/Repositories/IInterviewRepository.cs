using AppCore.Models;
using AppRepository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRepository.Repositories
{
    public interface IInterviewRepository : IGenericRepository<Interview>
    {
        Task CreateMeeting(Interview interview);
    }
}
