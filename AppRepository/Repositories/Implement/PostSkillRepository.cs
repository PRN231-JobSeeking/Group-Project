using AppCore;
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
    internal class PostSkillRepository : GenericRepository<PostSkillRequired>, IPostSkillRepository
    {
        public PostSkillRepository(Context context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
    }
}
