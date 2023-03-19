using AppCore;
using AppCore.Models;
using AppRepository.Generic;
using AppRepository.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppRepository.Repositories.Implement
{
    internal class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(Context context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
        public override async Task<IEnumerable<Post>> Get(Expression<Func<Post, bool>>? expression = null, params string[] includeProperties)
        {
            var result = await base.Get(expression, includeProperties);
            foreach (var item in result)
            {
                item.SkillRequired = _unitOfWork.PostSkillRepository.Get(c => c.PostId == item.Id).Result.ToList();
            }
            return result;
        }

        public async Task<Post> Get(int id)
        {
            return _context.Posts.Where(p => p.Id == id).FirstOrDefaultAsync().Result;
        }

        public async Task<IEnumerable<Post>> GetAll()
        {
            return _context.Posts.ToListAsync().Result;
        }
    }
}
