using AppCore;
using AppCore.Models;
using AppRepository.Generic;
using AppRepository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppRepository.Repositories.Implement
{
    internal class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(Context context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
        public override async Task<IEnumerable<Account>> Get(Expression<Func<Account, bool>>? expression = null, params string[] includeProperties)
        {
            var list = await base.Get(expression, includeProperties);
            foreach (var item in list)
            {
                var skills = await _unitOfWork.UserSkillRepository.Get(c => c.AccountId == item.Id);
                item.UserSkill = skills.ToList();
            }
            return list;
        }
    }
}
