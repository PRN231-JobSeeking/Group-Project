using AppCore;
using AppCore.Models;
using AppRepository.Generic;
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
        public AccountRepository(Context context) : base(context)
        {
        }

       
    }
}
